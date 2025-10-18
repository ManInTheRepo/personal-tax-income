using Tax.Domain.Schedules;
using Tax.Domain.Services;

using Xunit;

namespace Tax.Tests;

public class Au2024_25_BoundaryTests
{
    private readonly ProgressiveTaxCalculator _calc = new();
    
    [Theory]
    [InlineData(0,        0)]
    [InlineData(18_200,   0)]
    [InlineData(45_000,   4_288)]
    [InlineData(135_000,  31_288)]
    [InlineData(190_000,  51_638)]
    [InlineData(200_000,  56_138)]
    public void Boundaries_ReturnExpectedTax(decimal income, decimal expectedTax)
    {
        var actual = _calc.Calculate(income, Au_2024_25.Brackets);
        Assert.Equal(expectedTax, decimal.Round(actual, 2));
    }
}
