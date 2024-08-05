using Microsoft.EntityFrameworkCore;

namespace IdentityService.Data;

public static class DbInjector
{
    private const string ConfigurationName = "UsersDbConnection";
    public static void AddUsersDbContext(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddDbContext<IdentityDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString(ConfigurationName));
        });
    }
}
