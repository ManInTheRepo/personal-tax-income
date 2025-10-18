using Tax.Domain.Models;

namespace Tax.Domain.Schedules;

/// <summary>
/// Australia resident rates, FY 2024–25 (Stage 3).
/// No Medicare levy in this schedule.
/// Brackets:
/// 0–18,200 @ 0%
/// 18,201–45,000 @ 16%
/// 45,001–135,000 @ 30%
/// 135,001–190,000 @ 37%
/// 190,001+ @ 45%
/// Base taxes at lower bounds:
/// 18,200 => 0
/// 45,000 => 4,288
/// 135,000 => 31,288
/// 190,000 => 51,638
/// </summary>
public static class Au202425
{
    public static IReadOnlyList<TaxBracket> Brackets { get; } = new[]
    {
        new TaxBracket(0m,        18_200m, 0.00m, 0m),
        new TaxBracket(18_200m,   45_000m, 0.16m, 0m),
        new TaxBracket(45_000m,  135_000m, 0.30m, 4_288m),
        new TaxBracket(135_000m, 190_000m, 0.37m, 31_288m),
        new TaxBracket(190_000m,       null, 0.45m, 51_638m),
    };
}