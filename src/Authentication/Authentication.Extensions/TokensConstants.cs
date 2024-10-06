namespace Authentication.Configuration;

public static class TokensConstants
{
    public const string JwtCookieKey = "jwtToken";
    public const string RefreshCookieKey = "refreshToken";
    
    public const string JwtIssuer = "SecureApi";
    public const string JwtAudience = "SecureApiClient";
}