namespace IdentityService.Contracts;

public record UserToGet(Guid Id, string UserName, string? FirstName, string LastName, string Email, string? PhoneNumber);