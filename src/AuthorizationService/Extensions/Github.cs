using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace AuthorizationService.Extensions;

public static class Github
{
    public static void AddConfiguredGithub(this AuthenticationBuilder authBuilder, IConfiguration configuration)
    {
        authBuilder
            .AddGitHub("Github", options =>
            {
                options.ClientId = configuration["Github:ClientId"];
                options.ClientSecret = configuration["Github:ClientSecret"];
                options.Scope.Add("read:user");
                options.Scope.Add("user:email");
                options.Scope.Add("public_repo");
                options.Scope.Add("repo");
                options.Scope.Add("repo:status");
                options.Scope.Add("repo_deployment");
                options.Scope.Add("user:follow");

                options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
                options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
                options.ClaimActions.MapJsonKey("urn:github:avatar_url", "avatar_url");
                options.ClaimActions.MapJsonKey("urn:github:html_url", "html_url");
                options.ClaimActions.MapJsonKey("urn:github:email", "email");
            });
    }
}