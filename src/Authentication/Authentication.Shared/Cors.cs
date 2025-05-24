using Microsoft.Extensions.DependencyInjection;

namespace Authentication.Configuration;

public static class Cors
{
    public static IServiceCollection AddCorsPolicy(this IServiceCollection services, string policyName, string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            throw new ArgumentException("URL cannot be null or empty", nameof(url));
        }

        services.AddCors(options =>
        {
            options.AddPolicy(policyName, policy =>
            {
                policy
                    .WithOrigins(url)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        return services;
    }
}