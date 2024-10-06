using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Authentication.Configuration;
using Authentication.Configuration.Configurations;
using IdentityService.Data;
using IdentityService.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace IdentityService.Services;

public class TokenService : ITokenService
{
    private readonly IdentityAppContext _identityContext;
    private readonly UserManager<User> _userManager;

    public TokenService(IdentityAppContext context, UserManager<User> userManager)
    {
        _identityContext = context;
        _userManager = userManager;
    }

    public async Task<(string newRefreshToken, string newJwtToken)?> TryRefreshToken(string refreshTokenValue)
    {
        var user = _identityContext
            .Users
            .SingleOrDefault(u => u.RefreshTokens
                .Any(t => t.TokenValue == refreshTokenValue));

        if (user is null)
        {
            return null;
        }

        var refreshToken = user.RefreshTokens.Single(x => x.TokenValue == refreshTokenValue);

        if (!refreshToken.IsActive)
        {
            return null;
        }

        refreshToken.Revoked = DateTime.UtcNow;

        var newRefreshToken = GenerateRefreshToken();
        await SaveRefreshTokenAsync(user, newRefreshToken);

        var newjwtToken = await GenerateJwtToken(user);
        return new ValueTuple<string, string>(newRefreshToken.TokenValue, newjwtToken.TokenValue);
    }

    public async Task<Token> GenerateJwtToken(User user)
    {
        var tokenConfig = ConfigurationsManager.GetInstance().TokenConfiguration;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfig.SecretKey));
        var expiryMinutes = tokenConfig.ExpiryDays;

        var claims = await GetClaims(user);
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var expires = DateTime.UtcNow.AddMinutes(expiryMinutes);
        var tokenOptions = new JwtSecurityToken(
            issuer: TokensConstants.JwtIssuer,
            audience: TokensConstants.JwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
            signingCredentials: credentials
        );

        var token = new Token
            { ExpiryDate = expires, TokenValue = new JwtSecurityTokenHandler().WriteToken(tokenOptions) };
        return token;
    }

    public RefreshToken GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var generator = RandomNumberGenerator.Create();
        generator.GetBytes(randomNumber);

        return new RefreshToken
        {
            Created = DateTime.UtcNow,
            TokenValue = Convert.ToBase64String(randomNumber),
            ExpiryDate = DateTime.UtcNow.AddDays(10)
        };
    }

    public async Task<RefreshToken> GenerateRefreshTokenWithSave(User user)
    {
        var refreshToken = GenerateRefreshToken();
        await SaveRefreshTokenAsync(user, refreshToken);
        return refreshToken;
    }

    public ClaimsPrincipal GetClaimsFromExpiredToken(string token)
    {
        var jwtTokenParameters = JwtExtensions.GetTokenValidationParameters();

        var tokenHandler = new JwtSecurityTokenHandler();
        var claimsPrincipal = tokenHandler.ValidateToken(token, jwtTokenParameters, out var securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCulture))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return claimsPrincipal;
    }

    private async Task SaveRefreshTokenAsync(User user, RefreshToken refreshToken)
    {
        user.RefreshTokens.Add(refreshToken);
        _identityContext.Update(user);
        await _identityContext.SaveChangesAsync();
    }

    private async Task<IEnumerable<Claim>> GetClaims(User user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);

        var roleClaims = new List<Claim>();

        roleClaims.AddRange(roles.Select(role => new Claim("roles", role)));

        return new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.UserName!),
                new(JwtRegisteredClaimNames.Email, user.Email!),
                new("uid", user.Id.ToString()),
            }
            .Union(userClaims).Union(roleClaims);
    }
}

public interface ITokenService
{
    Task<(string newRefreshToken, string newJwtToken)?> TryRefreshToken(string refreshTokenValue);
    Task<Token> GenerateJwtToken(User user);
    RefreshToken GenerateRefreshToken();
    Task<RefreshToken> GenerateRefreshTokenWithSave(User user);
    ClaimsPrincipal GetClaimsFromExpiredToken(string token);
}