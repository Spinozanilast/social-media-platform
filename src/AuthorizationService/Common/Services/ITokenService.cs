using System.Security.Claims;
using AuthorizationService.Contracts;
using AuthorizationService.Entities;
using AuthorizationService.Entities.Tokens;

namespace AuthorizationService.Common.Services;

public interface ITokenService
{
    Task<Token> GenerateAccessTokenAsync(User user);

    RefreshToken GenerateRefreshToken(DeviceInfo deviceInfo, bool rememberUser);

    bool ValidateRefreshTokenAsync(User user, string refreshToken);

    Task RevokeRefreshTokenAsync(User user, string refreshToken);

    Task<int> RemoveExpiredRefreshTokensAsync(User user);

    bool IsTokenExpired(string token);
    ClaimsPrincipal? GetPrincipalFromToken(string token);

    Task StoreRefreshTokenAsync(User user, RefreshToken refreshToken);
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}