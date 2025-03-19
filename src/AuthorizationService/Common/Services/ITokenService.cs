using System.Security.Claims;
using IdentityService.Entities;
using IdentityService.Entities.Tokens;

namespace IdentityService.Common.Services;

public interface ITokenService
{
    Task<Token> GenerateAccessTokenAsync(User user);

    RefreshToken GenerateRefreshToken(string deviceId, string deviceName, string ipAddress, bool rememberUser);

    Task<bool> ValidateRefreshTokenAsync(User user, string refreshToken);

    Task RevokeRefreshTokenAsync(User user, string refreshToken);

    Task<int> RemoveExpiredRefreshTokensAsync(User user);

    Task StoreRefreshTokenAsync(User user, RefreshToken refreshToken);
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}