using IdentityService.Entities.Tokens;

namespace IdentityService.Common.Services;

public interface ICookieManager
{
    void SetAuthCookies(Token accessToken, RefreshToken refreshToken);

    void ClearAuthCookies();
    (string AccessToken, string RefreshToken) GetAuthCookies();
}