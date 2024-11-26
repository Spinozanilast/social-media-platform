using Microsoft.EntityFrameworkCore;

namespace ProfileService.Data;

public static class ProfileDbInjector
{
    private const string ConfigurationName = "PostgresConnection";

    public static void AddProfileDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ProfileDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString(ConfigurationName));
        });
    }

    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var dbcontext = scope.ServiceProvider.GetRequiredService<ProfileDbContext>();

        if (dbcontext.Database.GetPendingMigrations().Any())
        {
            return;
        }

        dbcontext.Database.Migrate();
    }
}