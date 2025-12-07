using Tax.Domain.Services;
using Tax.Domain.Schedules;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ITaxCalculator, ProgressiveTaxCalculator>();

var app = builder.Build();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.MapGet("/api/tax", (decimal taxableIncome, ITaxCalculator calc) =>
{
    if (taxableIncome < 0) return Results.BadRequest("Income cannot be negative.");

    // v1: fixed schedule AU 2024–25 resident, no levy
    var tax = calc.Calculate(taxableIncome, Au202425.Brackets);
    var netIncome = taxableIncome - tax;

    var resp = new { NetIncome = decimal.Round(netIncome, 2, MidpointRounding.AwayFromZero) };

    return Results.Ok(resp);
});

app.Run();