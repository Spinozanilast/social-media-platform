using Authentication.Configuration;
using Authentication.Configuration.Options;
using AuthorizationService.Common.Services;
using AuthorizationService.Entities.Tokens;
using Microsoft.Extensions.Options;

namespace AuthorizationService.Services;

public class CookieManager(IHttpContextAccessor httpContextAccessor, IOptions<JwtOptions> options) : ICookieManager
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly JwtOptions _options = options.Value;

    public void SetAuthCookies(Token accessToken, RefreshToken refreshToken)
    {
        var accessCookieOptions = CreateCookieOptions(expires: accessToken.Expires);
        var refreshCookieOptions = CreateCookieOptions(expires: refreshToken.Expires);

        SetCookie(_options.CookieNames[AuthCookieTypes.JwtCookie], accessToken.TokenValue,
            accessCookieOptions);
        SetCookie(_options.CookieNames[AuthCookieTypes.RefreshCookie], refreshToken.TokenValue,
            refreshCookieOptions);
    }

    public void ClearAuthCookies()
    {
        DeleteCookie(_options.CookieNames[AuthCookieTypes.JwtCookie]);
        DeleteCookie(_options.CookieNames[AuthCookieTypes.RefreshCookie]);
    }

    public (string AccessToken, string RefreshToken) GetAuthCookies()
    {
        var context = _httpContextAccessor.HttpContext;

        return new ValueTuple<string, string>(
            context?.Request.Cookies[_options.CookieNames[AuthCookieTypes.JwtCookie]],
            context?.Request.Cookies[_options.CookieNames[AuthCookieTypes.RefreshCookie]]);
    }

    private CookieOptions CreateCookieOptions(DateTimeOffset expires) => new()
    {
        HttpOnly = true,
        Expires = expires,
        SameSite = SameSiteMode.Strict,
        Secure = true
    };


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