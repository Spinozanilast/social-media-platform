using AuthorizationService.Entities.Tokens;

namespace AuthorizationService.Common.Services;

public interface ICookieManager
{
    void SetAuthCookies(Token accessToken, RefreshToken refreshToken);

    void ClearAuthCookies();
    (string AccessToken, string RefreshToken) GetAuthCookies();
}