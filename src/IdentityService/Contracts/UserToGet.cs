namespace IdentityService.Contracts;

public record UserToGet(Guid Id, string Username, string? FirstName, string LastName, string Email, string? PhoneNumber);