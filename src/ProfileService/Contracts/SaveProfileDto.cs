namespace ProfileService.Contracts;

public record SaveProfileDto(
    string? About,
    DateOnly? BirthDate,
    CountryDto? Country,
    List<string> Interests,
    List<string> References);