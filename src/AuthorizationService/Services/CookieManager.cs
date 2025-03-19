using Authentication.Configuration;
using IdentityService.Common.Services;
using IdentityService.Entities.Tokens;

namespace IdentityService.Services;

public class CookieManager(IHttpContextAccessor httpContextAccessor) : ICookieManager
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public void SetAuthCookies(Token accessToken, RefreshToken refreshToken)
    {
        var accessCookieOptions = CreateCookieOptions(expires: accessToken.Expires);
        var refreshCookieOptions = CreateCookieOptions(expires: refreshToken.Expires);

        SetCookie(TokensConstants.GetCookieKey(AuthCookieTypes.JwtCookie), accessToken.TokenValue,
            accessCookieOptions);
        SetCookie(TokensConstants.GetCookieKey(AuthCookieTypes.RefreshCookie), refreshToken.TokenValue,
            refreshCookieOptions);
    }

    public void ClearAuthCookies()
    {
        DeleteCookie(TokensConstants.GetCookieKey(AuthCookieTypes.JwtCookie));
        DeleteCookie(TokensConstants.GetCookieKey(AuthCookieTypes.RefreshCookie));
    }

    public (string AccessToken, string RefreshToken) GetAuthCookies()
    {
        var context = _httpContextAccessor.HttpContext;

        return new ValueTuple<string, string>(
            context?.Request.Cookies[TokensConstants.GetCookieKey(AuthCookieTypes.JwtCookie)],
            context?.Request.Cookies[TokensConstants.GetCookieKey(AuthCookieTypes.RefreshCookie)]);
    }

    private CookieOptions CreateCookieOptions(DateTimeOffset expires)
    {
        return new CookieOptions
        {
            HttpOnly = true,
            Expires = expires,
            SameSite = SameSiteMode.Strict,
            Secure = true
        };
    }

    private void SetCookie(string name, string value, CookieOptions options)
    {
        _httpContextAccessor.HttpContext?.Response.Cookies.Append(name, value, options);
    }

    private void DeleteCookie(string name)
    {
        _httpContextAccessor.HttpContext?.Response.Cookies.Delete(name, new CookieOptions
        {
            Path = "/"
        });
    }
}