namespace AuthorizationService.Contracts.Login;

public sealed record AuthResponse(
    Guid UserId,
    string UserName,
    string Email,
    IEnumerable<string> Roles,
    DateTime AccessTokenExpiry,
    string? AvatarUrl);