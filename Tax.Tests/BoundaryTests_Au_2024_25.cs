using FluentAssertions;
using Tax.Domain.Services;
using Tax.Domain.Schedules;

namespace Tax.Tests;

public class BoundaryTests_Au_2024_25
{
    private readonly ProgressiveTaxCalculator _calc = new();

    [Theory]
    [InlineData(0, 0)]
    [InlineData(18_200, 0)]
    public void LowIncome_NoTax(decimal income, decimal expectedTax)
    {
        var tax = _calc.Calculate(income, Au_2024_25.Brackets);
        tax.Should().Be(expectedTax);
    }

    [Fact]
    public void HighIncome_Example_200k()
    {
        // Tax to 190k = 51,638 ; remaining 10k at 45% = 4,500 ; total 56,138
        var tax = _calc.Calculate(200_000m, Au_2024_25.Brackets);
        tax.Should().Be(56_138m);
    }
}