using Microsoft.EntityFrameworkCore;

namespace StoriesService.Data;

public static class StoriesDbInjector
{
    private const string ConfigurationName = "PostgresConnection";

    public static void AddUsersDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<StoriesDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString(ConfigurationName));
        });
    }

    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var dbcontext = scope.ServiceProvider.GetRequiredService<StoriesDbContext>();

        if (dbcontext.Database.GetPendingMigrations().Any())
        {
            return;
        }

        dbcontext.Database.Migrate();
    }
}