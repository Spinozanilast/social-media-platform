using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Authentication.Configuration.Options;
using AuthorizationService.Common.Services;
using AuthorizationService.Contracts;
using AuthorizationService.Data;
using AuthorizationService.Entities;
using AuthorizationService.Entities.Tokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AuthorizationService.Services;

public class TokenService(
    IdentityAppContext context,
    UserManager<User> userManager,
    ICookieManager cookieManager,
    IOptions<JwtOptions> jwtOptions,
    ILogger<TokenService> logger)
    : ITokenService
{
    private readonly IdentityAppContext _context = context;
    private readonly UserManager<User> _userManager = userManager;
    private readonly ICookieManager _cookieManager = cookieManager;
    private readonly ILogger<TokenService> _logger = logger;
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    public async Task<Token> GenerateAccessTokenAsync(User user)
    {
        try
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
            var claims = await GetUserClaimsAsync(user);

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenExpiryMinutes),
                SigningCredentials = credentials,
                Issuer = _jwtOptions.ValidIssuer,
                Audience = _jwtOptions.ValidAudience,
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

    public RefreshToken GenerateRefreshToken(DeviceInfo deviceInfo, bool rememberUser)
    {
        var randomNumber = RandomNumberGenerator.GetBytes(64);

        return new RefreshToken
        {
            TokenValue = Convert.ToBase64String(randomNumber),
            Expires = rememberUser
                ? DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenExpiryDays)
                : DateTime.UtcNow.AddHours(_jwtOptions.ShortRefreshTokenExpiryHours),
            CreatedAt = DateTime.UtcNow,
            DeviceName = deviceInfo.DeviceName,
            IpAddress = deviceInfo.IpAddress,
            IsLongActive = rememberUser
        };
    }

    public bool ValidateRefreshTokenAsync(User user, string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken)) return false;

        var existingToken = user.RefreshTokens.FirstOrDefault(rt => rt.TokenValue == refreshToken);

        return existingToken is not null && existingToken.Expires > DateTime.UtcNow &&
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

        if (expiredTokens.Count == 0)
        {
            return 0;
        }

        foreach (var token in expiredTokens)
        {
            user.RefreshTokens.Remove(token);
        }

        await _userManager.UpdateAsync(user);
        return expiredTokens.Count;
    }

    public async Task StoreRefreshTokenAsync(User user, RefreshToken refreshToken)
    {
        var existingActiveTokens = user.RefreshTokens
            .Where(t => t.DeviceName == refreshToken.DeviceName && t.Revoked == null)
            .ToList();

        foreach (var token in existingActiveTokens)
        {
            token.Revoked = DateTime.UtcNow;
        }

        user.RefreshTokens.Add(refreshToken);

        while (user.RefreshTokens.Count > _jwtOptions.MaxRefreshTokensPerUser)
        {
            var oldestToken = user.RefreshTokens.MinBy(rt => rt.CreatedAt);
            if (oldestToken is not null) user.RefreshTokens.Remove(oldestToken);
        }

        await _userManager.UpdateAsync(user);
    }

    public ClaimsPrincipal? GetPrincipalFromToken(string token)
    {
        var parameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
            ValidateIssuer = _jwtOptions.ValidateIssuer,
            ValidateAudience = _jwtOptions.ValidateAudience,
            ValidateLifetime = true
        };

        try
        {
            return new JwtSecurityTokenHandler().ValidateToken(token, parameters, out _);
        }
        catch
        {
            _logger.LogError("Failed to validate token");
            return null;
        }
    }

    public bool IsTokenExpired(string token)
    {
        try
        {
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            return jwtToken.ValidTo < DateTime.UtcNow;
        }
        catch
        {
            return true;
        }
    }

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParams = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
            ValidateLifetime = false,
            ValidateIssuer = _jwtOptions.ValidateIssuer,
            ValidateAudience = _jwtOptions.ValidateAudience,
        };

        try
        {
            var principal =
                new JwtSecurityTokenHandler().ValidateToken(token, tokenValidationParams, out var validatedToken);

            if (validatedToken is not JwtSecurityToken jwt ||
                !jwt.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCulture))
            {
                throw new SecurityTokenException("Invalid token security algorithm");
            }

            if (jwt.ValidTo > DateTime.UtcNow)
            {
                throw new SecurityTokenException("Token is not expired");
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