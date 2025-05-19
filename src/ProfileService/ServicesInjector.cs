using ProfileService.Common.Repositories;
using ProfileService.Common.Services;
using ProfileService.Data;
using ProfileService.Repositories;
using ProfileService.Services;

namespace ProfileService;

public static class ServicesInjector
{
    public static IServiceCollection AddProfileServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddS3Client(configuration);
        services.AddScoped<IProfileImageService, ProfileImageService>();
        services.AddScoped<IProfileRepository, ProfileRepository>();
        services.AddScoped<ICountriesRepository, CountriesRepository>();
        services.Configure<ProfileImageStorageConfig>(configuration.GetSection("ProfileImageStorage"));

        return services;
    }
}