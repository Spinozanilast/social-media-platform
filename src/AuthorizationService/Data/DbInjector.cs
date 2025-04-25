using Microsoft.EntityFrameworkCore;

namespace AuthorizationService.Data;

public static class DbInjector
{
    private const string ConfigurationName = "PostgresConnection";
    public static void AddUsersDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IdentityAppContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString(ConfigurationName));
        });
    }

    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var dbcontext = scope.ServiceProvider.GetRequiredService<IdentityAppContext>();

        if (dbcontext.Database.GetPendingMigrations().Any())
        {
            return;
        }
        
        dbcontext.Database.Migrate();
    }
}
