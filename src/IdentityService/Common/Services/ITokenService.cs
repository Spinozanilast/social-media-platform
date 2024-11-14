using System.Security.Claims;
using IdentityService.Entities;
using IdentityService.Entities.Tokens;
using IdentityService.Helpers;

namespace IdentityService.Common.Services;

public interface ITokenService
{
    bool TryRevokeToken(User user, string refreshToken);
    void SetTokensInCookies(HttpResponse response, Token jwtToken, RefreshToken refreshToken);
    bool GetUsersRefreshTokenActivityStatus(User user, string refreshToken);
    Task<Result<TokenPair>> TryRefreshToken(User user, string refreshTokenValue);
    Task<Token> GenerateJwtToken(User user);
    RefreshToken GenerateRefreshToken();
    Task<RefreshToken> GenerateRefreshTokenWithSave(User user);
    ClaimsPrincipal GetClaimsFromExpiredToken(string token);
}