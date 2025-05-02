using Microsoft.AspNetCore.Authentication;

namespace AuthorizationService.Extensions;

public static class Github
{
    public static void AddConfiguredGithub(this AuthenticationBuilder authBuilder, IConfiguration configuration)
    {
        authBuilder
            .AddGitHub("Github", options =>
            {
                options.ClientId = configuration["GitHub:ClientId"];
                options.ClientSecret = configuration["GitHub:ClientSecret"];
                options.CallbackPath = "/api/v1/auth/github/callback";
                options.Scope.Add("user:email");
            });
    }
}