using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Authentication.Configuration;
using Authentication.Configuration.Configurations;
using IdentityService.Common.Services;
using IdentityService.Data;
using IdentityService.Entities;
using IdentityService.Entities.Tokens;
using IdentityService.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace IdentityService.Services;

public class TokenService : ITokenService
{
    private readonly IdentityAppContext _identityContext;
    private readonly UserManager<User> _userManager;
    private readonly ICookiesService _cookiesService;

    public TokenService(IdentityAppContext context, UserManager<User> userManager, ICookiesService cookiesService)
    {
        _identityContext = context;
        _userManager = userManager;
        _cookiesService = cookiesService;
    }

    public bool TryRevokeToken(User user, string refreshToken)
    {
        var usersToken = user.RefreshTokens.FirstOrDefault(token => token.TokenValue.Equals(refreshToken));
        if (usersToken is null)
        {
            return false;
        }

        usersToken.Revoked = DateTime.UtcNow;
        return true;
    }

    public void SetTokensInCookies(HttpResponse response, Token jwtToken, RefreshToken refreshToken)
    {
        _cookiesService.SetHttpOnlyCookies(response.Cookies, TokensConstants.GetCookieKey(AuthCookieTypes.JwtCookie),
            jwtToken.TokenValue,
            jwtToken.ExpiryDate);
        _cookiesService.SetHttpOnlyCookies(response.Cookies,
            TokensConstants.GetCookieKey(AuthCookieTypes.RefreshCookie), refreshToken.TokenValue,
            refreshToken.ExpiryDate);
    }

    public bool GetUsersRefreshTokenActivityStatus(User user, string refreshToken)
    {
        var usersToken = user.RefreshTokens.FirstOrDefault(token => token.TokenValue.Equals(refreshToken));

        return usersToken?.IsActive ?? false;
    }

    public async Task<Result<TokenPair>> TryRefreshToken(User user, string refreshTokenValue)
    {
        var refreshToken = user.RefreshTokens.Single(x => x.TokenValue == refreshTokenValue);

        if (!refreshToken.IsActive)
        {
            return Result<TokenPair>.Failure("Refresh token you have was expired or not exists");
        }

        refreshToken.Revoked = DateTime.UtcNow;

        var newRefreshToken = GenerateRefreshToken();
        await SaveRefreshTokenAsync(user, newRefreshToken);

        var newjwtToken = await GenerateJwtToken(user);
        return Result<TokenPair>.Success(new TokenPair
        (
            JwtToken: newjwtToken,
            RefreshToken: newRefreshToken
        ));
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