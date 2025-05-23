using AuthorizationService.Entities.OAuthInfos;

namespace AuthorizationService.Contracts.Login;

public sealed record AuthResponse(
    Guid UserId,
    string UserName,
    string Email,
    IEnumerable<string> Roles,
    DateTime AccessTokenExpiry,
    GithubInfo GithubInfo);