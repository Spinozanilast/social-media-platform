namespace Authentication.Configuration;

public static class TokensConstants
{
    private static readonly Dictionary<AuthCookieTypes, string> CookiesKeys = new()
    {
        { AuthCookieTypes.JwtCookie, JwtCookieKey },
        { AuthCookieTypes.RefreshCookie, RefreshCookieKey }
    };

    private const string JwtCookieKey = "access_token";
    private const string RefreshCookieKey = "refresh_token";

    public static string GetCookieKey(AuthCookieTypes authCookieType)
    {
        return CookiesKeys[authCookieType];
    }
}