namespace IdentityService.Helpers;

public static class HttpOnlyCookieAppender
{
    public static void AppendHttpOnlyCookie(this IResponseCookies responseCookies, string key, string value,
        DateTime? expires)
    {
        var httpOnlyCookieOptions = new CookieOptions
        {
            HttpOnly = true
        };

        if (expires.HasValue)
        {
            httpOnlyCookieOptions.Expires = expires.Value;
        }

        responseCookies.Append(key, value, new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.None,
            Secure = true
        });
    }
}