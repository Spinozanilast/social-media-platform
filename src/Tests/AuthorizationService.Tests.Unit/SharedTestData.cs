using Authentication.Configuration;
using Authentication.Configuration.Options;

namespace AuthorizationService.Tests.Unit;

public static class SharedTestData
{
    public static readonly JwtOptions JwtOptions =
        new()
        {
            SecretKey = "very-long-and-secure-secret-key-with-aaaaaaaaaaaaaaaaaaaaaaaaaaaaaa-a-hms512-cond",
            AccessTokenExpiryMinutes = 30,
            RefreshTokenExpiryDays = 7,
            ShortRefreshTokenExpiryHours = 2,
            MaxRefreshTokensPerUser = 5,
            CookieNames = new Dictionary<AuthCookieTypes, string>
            {
                [AuthCookieTypes.JwtCookie] = "access_token",
                [AuthCookieTypes.RefreshCookie] = "refresh_token",
            },
            ValidIssuer = "test-issuer",
            ValidAudience = "test-audience"
        };
}