namespace IdentityService.Contracts.Users;

public record UserDto(Guid Id, string UserName, string? FirstName, string LastName, string Email, string? PhoneNumber);