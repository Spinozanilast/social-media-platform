namespace AuthorizationService.Entities.OAuthInfos;

public class GithubInfo
{
    public string GithubId { get; set; }
    public string ProfileUrl { get; set; }
    public string GithubUsername { get; set; }
    public string GithubEmail { get; set; }
    public string? AvatarUrl { get; set; }
}