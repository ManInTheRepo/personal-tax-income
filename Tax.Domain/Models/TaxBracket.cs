namespace Tax.Domain.Models;

public sealed record TaxBracket(
    decimal LowerInclusive,
    decimal? UpperInclusive,
    decimal MarginalRate,   // e.g. 0.16m for 16%
    decimal BaseTaxAtLower  // tax payable at LowerInclusive
);