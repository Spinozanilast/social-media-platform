using System.Security.Claims;
using IdentityService.Entities;
using IdentityService.Entities.Tokens;

namespace IdentityService.Services;

public interface ITokenService
{
    Task<TokenPair?> TryRefreshToken(User user, string refreshTokenValue);
    Task<Token> GenerateJwtToken(User user);
    RefreshToken GenerateRefreshToken();
    Task<RefreshToken> GenerateRefreshTokenWithSave(User user);
    ClaimsPrincipal GetClaimsFromExpiredToken(string token);
    
}