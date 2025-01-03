namespace IdentityService.Contracts;

public record UserUpdateDto(string? UserName, string? FirstName, string? LastName, string? Email, string? PhoneNumber);