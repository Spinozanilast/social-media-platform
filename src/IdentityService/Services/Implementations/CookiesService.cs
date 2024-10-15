using Authentication.Configuration;
using IdentityService.Utilities;

namespace IdentityService.Services.Implementations;

public class CookiesService : ICookiesService
{
    private readonly IEnumerable<string> _authCookiesKeys =
        [TokensConstants.JwtCookieKey, TokensConstants.RefreshCookieKey];

    public void ExpireAuthHttpOnlyCookies(HttpRequest request, HttpResponse response)
    {
        foreach (var cookieKey in _authCookiesKeys)
        {
            if (request.Cookies.TryGetValue(cookieKey, out _))
            {
                response.Cookies.Delete(cookieKey);
            }
        }
    }

    public void SetHttpOnlyCookies(HttpResponse response, string key, string value, DateTime expiredTime)
    {
        response.Cookies.AppendHttpOnlyCookie(key, value, expiredTime);
    }
}