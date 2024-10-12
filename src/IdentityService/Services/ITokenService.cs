using System.Security.Claims;
using IdentityService.Entities;
using IdentityService.Entities.Tokens;
using IdentityService.Utilities;

namespace IdentityService.Services;

public interface ITokenService
{
    bool TryRevokeToken(User user, string refreshToken);
    bool GetUsersRefreshTokenActivityStatus(User user, string refreshToken);
    Task<Result<TokenPair>> TryRefreshToken(User user, string refreshTokenValue);
    Task<Token> GenerateJwtToken(User user);
    RefreshToken GenerateRefreshToken();
    Task<RefreshToken> GenerateRefreshTokenWithSave(User user);
    ClaimsPrincipal GetClaimsFromExpiredToken(string token);
}