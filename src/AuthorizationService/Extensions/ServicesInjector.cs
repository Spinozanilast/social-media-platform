using AuthorizationService.Common.Services;
using AuthorizationService.Services;

namespace AuthorizationService.Extensions;

public static class ServicesInjector
{
    public static IServiceCollection AddAuthorizationServices(this IServiceCollection services, IConfiguration _)
    {
        services.AddScoped<ITokenService, TokenService>();
        services.AddTransient<ICookieManager, CookieManager>();

        return services;
    }
}