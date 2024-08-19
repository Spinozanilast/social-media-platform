using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Authentication.Configuration.Configurations;
using IdentityService.Entities;
using Microsoft.IdentityModel.Tokens;

namespace IdentityService.Services;

public class TokenService
{
    private readonly IConfiguration _configuration;
    private readonly IConfiguration _userManager;
 
    public TokenService(IConfiguration configuration, IConfiguration userManager)
    {
        _configuration = configuration;
        _userManager = userManager;
    }

    public string GenerateToken(User user)
    {
        var tokenConfig = ConfigurationsManager.GetInstance().TokenConfiguration;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfig.SecretKey));
        var expiryDays = tokenConfig.ExpiryDays;
        
        var claims = GetClaims(user);
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
        
        var tokenOptions = new JwtSecurityToken(
            issuer: null,
            audience: null,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(expiryDays),
            signingCredentials: credentials
        );
        
        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

    private IEnumerable<Claim> GetClaims(User user) =>
        new List<Claim>()
        {
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.Name, user.UserName!),
        };
}