using ProfileService.Entities;

namespace ProfileService.Contracts;

public record SaveProfileDto(
    Guid UserId,
    string? About,
    string? Anything,
    DateOnly? BirthDate,
    CountryDto Country,
    List<string> Interests,
    List<string> References);