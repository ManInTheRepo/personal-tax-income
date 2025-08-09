using Tax.Domain.Services;
using Tax.Domain.Schedules;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ITaxCalculator, ProgressiveTaxCalculator>();

var app = builder.Build();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.MapPost("/calculate", (CalculateRequest req, ITaxCalculator calc) =>
{
    if (req.TaxableIncome < 0) return Results.BadRequest("Income cannot be negative.");

    // v1: fixed schedule AU 2024–25 resident, no levy
    var tax = calc.Calculate(req.TaxableIncome, Au_2024_25.Brackets);
    var effectiveRate = req.TaxableIncome == 0 ? 0 : tax / req.TaxableIncome;

    var resp = new CalculateResponse
    {
        Income = req.TaxableIncome,
        Tax = tax,
        EffectiveRate = decimal.Round(effectiveRate, 6, MidpointRounding.AwayFromZero)
    };

    return Results.Ok(resp);
});

app.Run();

public sealed record CalculateRequest(decimal TaxableIncome);

public sealed record CalculateResponse
{
    public decimal Income { get; init; }
    public decimal Tax { get; init; }
    public decimal EffectiveRate { get; init; } // e.g. 0.280690
}