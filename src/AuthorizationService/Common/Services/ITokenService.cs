using System.Security.Claims;
using AuthorizationService.Entities;
using AuthorizationService.Entities.Tokens;

namespace AuthorizationService.Common.Services;

public interface ITokenService
{
    Task<Token> GenerateAccessTokenAsync(User user);

    RefreshToken GenerateRefreshToken(string deviceId, string deviceName, string ipAddress, bool rememberUser);

    bool ValidateRefreshTokenAsync(User user, string refreshToken);

    Task RevokeRefreshTokenAsync(User user, string refreshToken);

    Task<int> RemoveExpiredRefreshTokensAsync(User user);

    Task StoreRefreshTokenAsync(User user, RefreshToken refreshToken);
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}