namespace AuthorizationService.Contracts.Users;

public record UserUpdateRequest(
    string? UserName,
    string? FirstName,
    string? LastName,
    string? Email,
    string? PhoneNumber);