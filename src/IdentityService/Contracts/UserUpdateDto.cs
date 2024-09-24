namespace IdentityService.Contracts;

public record UserUpdateDto(string? Username, string? FirstName, string? LastName, string? Email, string? PhoneNumber);