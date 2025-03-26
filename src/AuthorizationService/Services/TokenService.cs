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
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace IdentityService.Services;

public class TokenService(
    IdentityAppContext context,
    UserManager<User> userManager,
    ICookieManager cookieManager,
    ILogger<TokenService> logger)
    : ITokenService
{
    private readonly IdentityAppContext _context = context;
    private readonly UserManager<User> _userManager = userManager;
    private readonly ICookieManager _cookieManager = cookieManager;
    private readonly ILogger<TokenService> _logger = logger;

    public async Task<Token> GenerateAccessTokenAsync(User user)
    {
        try
        {
            var tokenConfig = ConfigurationsManager.GetInstance().TokenConfiguration;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfig.SecretKey));

            var expiryDays = tokenConfig.ExpiryDays;
            var claims = await GetUserClaimsAsync(user);
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(expiryDays - 5),
                SigningCredentials = credentials,
                Issuer = null,
                Audience = null
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new Token
            {
                TokenValue = tokenHandler.WriteToken(token),
                Expires = tokenDescriptor.Expires.Value
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate access token for user {UserId}", user.Id);
            throw new SecurityTokenException("Token generation failed");
        }
    }

    public RefreshToken GenerateRefreshToken(string deviceId, string deviceName, string ipAddress, bool rememberUser)
    {
        using var generator = RandomNumberGenerator.Create();
        var randomNumber = new byte[64];
        generator.GetBytes(randomNumber);

        var expiryDate = rememberUser ? DateTime.UtcNow.AddDays(10) : DateTime.UtcNow.AddHours(1);

        return new RefreshToken
        {
            CreatedAt = DateTime.UtcNow,
            TokenValue = Convert.ToBase64String(randomNumber),
            Expires = DateTime.UtcNow.AddDays(10),
            DeviceId = deviceId,
            DeviceName = deviceName,
            IpAddress = ipAddress
        };
    }

    public async Task<bool> ValidateRefreshTokenAsync(User user, string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken)) return false;

        var existingToken = user.RefreshTokens.FirstOrDefault(rt => rt.TokenValue == refreshToken);

        return existingToken is not null && existingToken.Expires < DateTime.UtcNow &&
               existingToken.IsActive;
    }

    public async Task RevokeRefreshTokenAsync(User user, string refreshToken)
    {
        var existingToken = user.RefreshTokens.FirstOrDefault(rt => rt.TokenValue == refreshToken);

        if (existingToken is not null && existingToken.IsActive)
        {
            existingToken.Revoked = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<int> RemoveExpiredRefreshTokensAsync(User user)
    {
        var expiredTokens = user.RefreshTokens.Where(rt => !rt.IsActive).ToList();

        foreach (var token in expiredTokens)
        {
            user.RefreshTokens.Remove(token);
        }

        await _userManager.UpdateAsync(user);
        return expiredTokens.Count;
    }

    public async Task StoreRefreshTokenAsync(User user, RefreshToken refreshToken)
    {
        user.RefreshTokens.Add(refreshToken);

        while (user.RefreshTokens.Count > 10)
        {
            var oldestToken = user.RefreshTokens.OrderBy(rt => rt.CreatedAt).First();
            user.RefreshTokens.Remove(oldestToken);
        }

        await _userManager.UpdateAsync(user);
    }

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParams = JwtExtensions.GetTokenValidationParameters();

        try
        {
            var principal =
                new JwtSecurityTokenHandler().ValidateToken(token, tokenValidationParams, out var validatedToken);

            if (validatedToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCulture))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to validate expired token");
            return null;
        }
    }

    private async Task<IEnumerable<Claim>> GetUserClaimsAsync(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var userClaims = await _userManager.GetClaimsAsync(user);
        var userRoles = await _userManager.GetRolesAsync(user);

        claims.AddRange(userClaims);
        claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

        return claims;
    }
}