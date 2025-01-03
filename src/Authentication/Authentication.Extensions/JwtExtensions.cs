using System.Text;
using Authentication.Configuration.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Authentication.Configuration;

public static class JwtExtensions
{
    public static void AddJwtConfiguration(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = GetTokenValidationParameters();
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    if (context.Request.Cookies.TryGetValue(TokensConstants.GetCookieKey(AuthCookieTypes.JwtCookie),
                            out var accessToken))
                    {
                        if (!string.IsNullOrEmpty(accessToken))
                        {
                            context.Token = accessToken;
                        }
                        else
                        {
                            context.Response.Headers["Token-Error"] = "Token is empty";
                        }
                    }
                    else
                    {
                        context.Response.Headers["Token-Error"] = "Token not found";
                    }

                    return Task.CompletedTask;
                },
                OnAuthenticationFailed = context =>
                {
                    context.Response.Headers["Token-Error"] = "Authentication failed: " + context.Exception.Message;
                    return Task.CompletedTask;
                }
            };
        });
    }

    public static TokenValidationParameters GetTokenValidationParameters()
    {
        var secretKey = ConfigurationsManager.GetInstance().TokenConfiguration.SecretKey;
        return new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(secretKey)),
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    }
}