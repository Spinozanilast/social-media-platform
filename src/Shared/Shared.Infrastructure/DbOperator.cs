using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Infrastructure;

public class DbOperator<TDbContext>(string connectionString)
    where TDbContext : DbContext
{
    private const string DefaultConfigurationName = "PostgresConnection";

    private readonly string _connectionString = connectionString;

    public DbOperator() : this(DefaultConfigurationName)
    {
    }

    public void AddDbContext(IServiceCollection services, IConfiguration configuration,
        Action<DbContextOptionsBuilder>? optionsAction = null)
    {
        services.AddDbContext<TDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString(_connectionString));
            optionsAction?.Invoke(options);
        });
    }

    public void AddDbContextWithSnakeNamingConvention(IServiceCollection services, IConfiguration configuration) =>
        AddDbContext(services, configuration, optionsAction: o => o.UseSnakeCaseNamingConvention());

    public void AddDbContextWithSnakeNamingConvention(IServiceCollection services, IConfiguration configuration,
        Action<DbContextOptionsBuilder> optionsAction) =>
        AddDbContext(services, configuration, optionsAction: o =>
        {
            o.UseSnakeCaseNamingConvention();
            optionsAction.Invoke(o);
        });

    public async Task ApplyMigrations(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var dbcontext = scope.ServiceProvider.GetRequiredService<TDbContext>();

        if ((await dbcontext.Database.GetPendingMigrationsAsync().ConfigureAwait(false)).Any())
        {
            await dbcontext.Database.MigrateAsync();
        }
    }
}