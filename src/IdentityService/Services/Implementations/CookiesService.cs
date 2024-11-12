using Authentication.Configuration;
using IdentityService.Utilities;

namespace IdentityService.Services.Implementations;

public class CookiesService : ICookiesService
{
    private readonly IEnumerable<string> _authCookiesKeys =
        TokensConstants.GetCookiesKeys();

    public void ExpireAuthHttpOnlyCookies(IRequestCookieCollection requestCookies, IResponseCookies responseCookies)
    {
        foreach (var cookieKey in _authCookiesKeys)
        {
            if (requestCookies.TryGetValue(cookieKey, out _))
            {
                responseCookies.Delete(cookieKey);
            }
        }
    }

    public void SetHttpOnlyCookies(IResponseCookies responseCookies, string key, string value, DateTime expiredTime)
    {
        responseCookies.AppendHttpOnlyCookie(key, value, expiredTime);
    }

    public bool JwtTokenExistsInRequest(IRequestCookieCollection requestCookies, AuthCookieTypes authCookieType)
    {
        return requestCookies.ContainsKey(TokensConstants.GetCookieKey(authCookieType));
    }

    public bool TryGetCookie(IRequestCookieCollection requestCookies, AuthCookieTypes authCookieType,
        out string? cookie)
    {
        return requestCookies.TryGetValue(TokensConstants.GetCookieKey(authCookieType), out cookie);
    }
}