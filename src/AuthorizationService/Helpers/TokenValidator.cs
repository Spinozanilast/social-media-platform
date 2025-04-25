using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace AuthorizationService.Helpers;

public class TokenValidator(TokenValidationParameters validationParameters)
{
    private readonly TokenValidationParameters _tokenValidationParameters = validationParameters;

    public bool IsTokenValidByClaim(string token, string claimType, string expectedClaimValue)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var securityToken);
        
        if (securityToken is not JwtSecurityToken)
        {
            throw new SecurityTokenException("Invalid token");
        }

        var claim = principal.Claims.FirstOrDefault(c =>
            c.Properties.Values.FirstOrDefault() == claimType && c.Value == expectedClaimValue);

        return claim is not null;
    }
}