using Tax.Domain.Models;

namespace Tax.Domain.Services;

public interface ITaxCalculator
{
    decimal Calculate(decimal taxableIncome, IReadOnlyList<TaxBracket> schedule);
}

public sealed class ProgressiveTaxCalculator : ITaxCalculator
{
    public decimal Calculate(decimal taxableIncome, IReadOnlyList<TaxBracket> schedule)
    {
        if (taxableIncome <= 0) return 0m;

        // find the bracket the income falls into
        var bracket = schedule.First(b =>
            taxableIncome >= b.LowerInclusive &&
            (b.UpperInclusive is null || taxableIncome <= b.UpperInclusive.Value));

        var upperForCalc = bracket.UpperInclusive ?? taxableIncome;
        var taxableAtMarginal = taxableIncome - bracket.LowerInclusive;

        var tax = bracket.BaseTaxAtLower + (taxableAtMarginal * bracket.MarginalRate);
        return decimal.Round(tax, 2, MidpointRounding.AwayFromZero);
    }
}