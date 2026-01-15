using System.Text.Json;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Context;
using Tax.Domain.Services;
using Tax.Domain.Schedules;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog from configuration and optionally Seq
builder.Host.UseSerilog((ctx, services, lc) =>
{
    lc.ReadFrom.Configuration(ctx.Configuration)
      .Enrich.FromLogContext()
      .Enrich.WithProperty("Application", "Tax.Api");

    var seqEnabled = ctx.Configuration.GetValue<bool>("Logging:Seq:Enabled", false);
    if (seqEnabled)
    {
        var seqUrl = ctx.Configuration.GetValue<string>("Logging:Seq:Url");
        if (!string.IsNullOrEmpty(seqUrl)) lc.WriteTo.Seq(seqUrl);
    }
});

builder.Services.AddSingleton<ITaxCalculator, ProgressiveTaxCalculator>();

// OpenAPI/Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Personal Tax Income API",
        Version = "v1",
        Description = "Compute tax and net income for AU 2024-25 resident schedule (no levy)."
    });
});

// Use camelCase for JSON property names across the API
builder.Services.ConfigureHttpJsonOptions(opts =>
{
    opts.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

// Read HTTP logging configuration
var httpBodyLimit = builder.Configuration.GetValue<int>("Logging:HttpBodyLimit", 4096);
var httpBodySamplingRate = builder.Configuration.GetValue<double>("Logging:HttpBodySamplingRate", 0.1);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

static bool DeterministicSample(string traceId, double rate)
{
    if (rate <= 0) return false;
    if (rate >= 1) return true;

    // Hash the traceId and map to [0,1)
    using var sha = SHA256.Create();
    var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(traceId ?? string.Empty));
    // Use first 8 bytes for a deterministic 64-bit value
    if (hash.Length < 8) return false;
    var val = BitConverter.ToUInt64(hash, 0);
    // normalize to [0,1)
    var sampleScore = val / (double)ulong.MaxValue;
    return sampleScore < rate;
}

// Detailed HTTP logging middleware with configurable body limit and sampling (deterministic)
app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
    var traceId = context.TraceIdentifier;
    var sensitiveHeaders = new[] { "Authorization", "Cookie", "Set-Cookie" };
    var bodyLimit = httpBodyLimit;

    // Determine if we should capture full bodies for this request
    var forced = context.Request.Headers.ContainsKey("X-Debug-Log") || context.Request.Query.ContainsKey("debugLog");
    var sampled = DeterministicSample(traceId, httpBodySamplingRate);
    var captureBody = forced || sampled;

    using (LogContext.PushProperty("TraceId", traceId))
    {
        try
        {
            // Log request line
            logger.LogInformation("HTTP Request {Method} {Path}", context.Request.Method, context.Request.Path + context.Request.QueryString);

            // Log request headers (excluding sensitive)
            foreach (var header in context.Request.Headers.Where(h => !sensitiveHeaders.Contains(h.Key, StringComparer.OrdinalIgnoreCase)))
            {
                logger.LogDebug("Request header {Header}: {Value}", header.Key, string.Join(',', header.Value.ToArray()));
            }

            if (captureBody && (context.Request.ContentLength > 0 || context.Request.Body.CanSeek))
            {
                context.Request.EnableBuffering();
                using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
                var buffer = new char[bodyLimit];
                var read = await reader.ReadBlockAsync(buffer, 0, bodyLimit);
                var requestBody = new string(buffer, 0, read);
                if (context.Request.ContentLength.HasValue && context.Request.ContentLength.Value > read)
                {
                    requestBody += "...(truncated)";
                }
                context.Request.Body.Position = 0;
                if (!string.IsNullOrEmpty(requestBody))
                    logger.LogDebug("Request body: {Body}", requestBody);
            }

            if (captureBody)
            {
                // Capture response
                var originalBodyStream = context.Response.Body;
                await using var responseBody = new MemoryStream();
                context.Response.Body = responseBody;

                var sw = Stopwatch.StartNew();
                await next();
                sw.Stop();

                context.Response.Body.Seek(0, SeekOrigin.Begin);
                using var respReader = new StreamReader(context.Response.Body);
                var responseText = await respReader.ReadToEndAsync();
                if (responseText.Length > bodyLimit)
                    responseText = responseText.Substring(0, bodyLimit) + "...(truncated)";

                context.Response.Body.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);

                // Log response status and headers (excluding sensitive)
                logger.LogInformation("HTTP Response {StatusCode} {ElapsedMs}ms", context.Response.StatusCode, sw.ElapsedMilliseconds);
                foreach (var header in context.Response.Headers.Where(h => !sensitiveHeaders.Contains(h.Key, StringComparer.OrdinalIgnoreCase)))
                {
                    logger.LogDebug("Response header {Header}: {Value}", header.Key, string.Join(',', header.Value.ToArray()));
                }

                if (!string.IsNullOrEmpty(responseText))
                    logger.LogDebug("Response body: {Body}", responseText);
            }
            else
            {
                // Do not capture bodies; just execute pipeline and log summary
                var sw = Stopwatch.StartNew();
                await next();
                sw.Stop();

                logger.LogInformation("HTTP Response {StatusCode} {ElapsedMs}ms", context.Response.StatusCode, sw.ElapsedMilliseconds);
                foreach (var header in context.Response.Headers.Where(h => !sensitiveHeaders.Contains(h.Key, StringComparer.OrdinalIgnoreCase)))
                {
                    logger.LogDebug("Response header {Header}: {Value}", header.Key, string.Join(',', header.Value.ToArray()));
                }
            }
        }
        catch (Exception ex)
        {
            var loggerEx = context.RequestServices.GetRequiredService<ILogger<Program>>();
            loggerEx.LogError(ex, "Error in HTTP logging middleware");
            throw;
        }
    }
});

// Global exception handler that returns ProblemDetails and logs
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exFeature = context.Features.Get<IExceptionHandlerFeature>();
        var ex = exFeature?.Error;
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Unhandled exception while processing request {Method} {Path}", context.Request.Method, context.Request.Path);

        var pd = new ProblemDetails
        {
            Type = "https://httpstatuses.com/500",
            Title = "An unexpected error occurred",
            Detail = "An unexpected error occurred while processing your request.",
            Status = StatusCodes.Status500InternalServerError,
            Instance = context.Request.Path
        };

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsJsonAsync(pd);
    });
});

// Middleware to translate 400 responses into structured ProblemDetails for API endpoints
app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == StatusCodes.Status400BadRequest &&
        context.Request.Path.StartsWithSegments("/api") &&
        !context.Response.HasStarted)
    {
        var pd = new ProblemDetails
        {
            Type = "https://httpstatuses.com/400",
            Title = "Bad Request",
            Detail = "The request is invalid. Check the payload and parameters.",
            Status = StatusCodes.Status400BadRequest,
            Instance = context.Request.Path
        };

        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsJsonAsync(pd);
    }
});

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.MapGet("/api/tax", (HttpContext httpContext, decimal? taxableIncome, ITaxCalculator calc, ILogger<Program> logger) =>
{
    var traceId = httpContext.TraceIdentifier;
    logger.LogInformation("Received tax request {TraceId} taxableIncome={TaxableIncome}", traceId, taxableIncome);

    if (!taxableIncome.HasValue)
    {
        var pd = new ProblemDetails
        {
            Type = "https://httpstatuses.com/400",
            Title = "Bad Request",
            Detail = "Missing required query parameter 'taxableIncome'.",
            Status = StatusCodes.Status400BadRequest,
            Instance = httpContext.Request.Path
        };

        logger.LogWarning("Validation failed for request {TraceId}: missing taxableIncome", traceId);
        return Results.Problem(title: pd.Title, detail: pd.Detail, statusCode: pd.Status, instance: pd.Instance, type: pd.Type);
    }

    if (taxableIncome.Value < 0)
    {
        var pd = new ProblemDetails
        {
            Type = "https://httpstatuses.com/400",
            Title = "Invalid Input",
            Detail = "Income cannot be negative.",
            Status = StatusCodes.Status400BadRequest,
            Instance = httpContext.Request.Path
        };

        logger.LogWarning("Validation failed for request {TraceId}: negative income {Income}", traceId, taxableIncome.Value);
        return Results.Problem(title: pd.Title, detail: pd.Detail, statusCode: pd.Status, instance: pd.Instance, type: pd.Type);
    }

    // v1: fixed schedule AU 2024–25 resident, no levy
    var tax = calc.Calculate(taxableIncome.Value, Au202425.Brackets);
    var netIncome = taxableIncome.Value - tax;
    var effectiveRate = taxableIncome.Value == 0 ? 0 : tax / taxableIncome.Value;

    var resp = new GetTaxResponse
    {
        Income = decimal.Round(taxableIncome.Value, 2, MidpointRounding.AwayFromZero),
        Tax = decimal.Round(tax, 2, MidpointRounding.AwayFromZero),
        NetIncome = decimal.Round(netIncome, 2, MidpointRounding.AwayFromZero),
        EffectiveRate = decimal.Round(effectiveRate, 6, MidpointRounding.AwayFromZero)
    };

    logger.LogInformation("Returning tax response {TraceId} income={Income} tax={Tax} netIncome={NetIncome}", traceId, resp.Income, resp.Tax, resp.NetIncome);

    // add trace id header for observability
    httpContext.Response.Headers["X-Trace-Id"] = traceId;

    return Results.Ok(resp);
});

app.Run();

public sealed record GetTaxResponse
{
    public decimal Income { get; init; }
    public decimal Tax { get; init; }
    public decimal NetIncome { get; init; }
    public decimal EffectiveRate { get; init; }
}