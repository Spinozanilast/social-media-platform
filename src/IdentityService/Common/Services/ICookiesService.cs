using Authentication.Configuration;

namespace IdentityService.Common.Services;

public interface ICookiesService
{
    void ExpireAuthHttpOnlyCookies(IRequestCookieCollection requestCookies, IResponseCookies responseCookies);
    void SetHttpOnlyCookies(IResponseCookies requestCookies, string key, string value, DateTime expiredTime);
    bool JwtTokenExistsInRequest(IRequestCookieCollection requestCookies, AuthCookieTypes authCookieType);
    bool TryGetCookie(IRequestCookieCollection requestCookies, AuthCookieTypes authCookieType, out string? cookie);
}