namespace Authentication.Configuration.Options;

public class JwtOptions
{
    public const string SectionName = "Jwt";
    public string SecretKey { get; set; } = string.Empty;
    public int AccessTokenExpiryMinutes { get; set; } = 60;
    public int RefreshTokenExpiryDays { get; set; } = 10;
    public int ShortRefreshTokenExpiryHours { get; set; } = 10;
    public int MaxRefreshTokensPerUser { get; set; } = 10;
    public bool ValidateIssuer { get; set; }
    public bool ValidateAudience { get; set; }
    public string ValidIssuer { get; set; } = string.Empty;
    public string ValidAudience { get; set; } = string.Empty;
    public Dictionary<AuthCookieTypes, string> CookieNames { get; set; } = new();

    public GithubOAuthConfig GithubConfig { get; set; } = new();
}