using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;

namespace Authentication.Configuration;

public static class JwtExtensions
{
    public static AuthenticationBuilder AddJwtAuthentication(this IServiceCollection services)
    {
        services.ConfigureOptions<JwtBearerOptionsSetup>();
        
        return services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer();
    }
}