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
            options.Events = new JwtBearerEvents()
            {
                OnMessageReceived = context =>
                {
                    context.Request.Cookies.TryGetValue(TokensConstants.GetCookieKey(AuthCookieTypes.JwtCookie),
                        out var accessToken);
                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        context.Token = accessToken;
                    }

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
            ValidIssuer = TokensConstants.JwtIssuer,
            ValidAudience = TokensConstants.JwtAudience,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidateIssuer = true,
            ValidateAudience = true
        };
    }
}