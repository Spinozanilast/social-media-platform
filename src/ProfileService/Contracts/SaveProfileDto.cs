namespace ProfileService.Contracts;

public record SaveProfileDto(
    string? About,
    string? Anything,
    DateOnly? BirthDate,
    CountryDto? Country,
    List<string> Interests,
    List<string> References);