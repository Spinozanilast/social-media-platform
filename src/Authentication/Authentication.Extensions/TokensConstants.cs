namespace Authentication.Configuration;

public static class TokensConstants
{
    private static readonly Dictionary<AuthCookieTypes, string> CookiesKeys = new()
    {
        { AuthCookieTypes.JwtCookie, JwtCookieKey },
        { AuthCookieTypes.RefreshCookie, RefreshCookieKey }
    };

    private const string JwtCookieKey = "jwtToken";
    private const string RefreshCookieKey = "refreshToken";

    public const string JwtIssuer = "SecureApi";
    public const string JwtAudience = "SecureApiClient";

    public static string GetCookieKey(AuthCookieTypes authCookieType)
    {
        return CookiesKeys[authCookieType];
    }

    public static string[] GetCookiesKeys() => CookiesKeys.Values.Select(v => v).ToArray();
}