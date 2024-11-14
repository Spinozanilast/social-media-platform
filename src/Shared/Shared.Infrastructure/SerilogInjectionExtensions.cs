using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Shared.Infrastructure;

public static class SerilogInjectionExtensions
{
    public static void AddConfiguredSerilog(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSerilog((cfg, loggerConfig) =>
        {
            loggerConfig.WriteTo.Console();
            loggerConfig.ReadFrom.Configuration(configuration);
        });
    }
}