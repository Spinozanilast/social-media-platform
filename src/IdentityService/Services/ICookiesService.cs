namespace IdentityService.Services;

public interface ICookiesService
{
    void ExpireAuthHttpOnlyCookies(HttpRequest request, HttpResponse response);
    void SetHttpOnlyCookies(HttpResponse response, string key, string value, DateTime expiredTime);
}