using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure.Configurations;

namespace Shared.Infrastructure;

public static class RabbitMqInjectExtensions
{
    public static void AddMassTransitConfigured(
        this IServiceCollection services,
        RabbitMqConfiguration? rabbitMqConfig = null,
        Action<IBusRegistrationConfigurator>? configureBus = null,
        Action<IBusRegistrationContext, IRabbitMqBusFactoryConfigurator>? configureRabbitMq = null)
    {
        services.AddMassTransit(x =>
            {
                configureBus?.Invoke(x);

                x.UsingRabbitMq((context, rqConfig) =>
                {
                    rqConfig.GetStandardMessageRetryConfiguration();
                    rqConfig.GetRabbitMqHostConfiguration(rabbitMqConfig);

                    rqConfig.ConfigureEndpoints(context);

                    configureRabbitMq?.Invoke(context, rqConfig);
                });
            }
        );
    }

    private static void GetStandardMessageRetryConfiguration(this IConsumePipeConfigurator configurator)
    {
        configurator.UseMessageRetry(cfg => cfg.Interval(5, TimeSpan.FromSeconds(10)));
    }

    private static void GetRabbitMqHostConfiguration(
        this IRabbitMqBusFactoryConfigurator configurator, RabbitMqConfiguration rabbitMqCfg)
    {
        var areCfgPropsSetted = rabbitMqCfg.Username.Length > 0 && rabbitMqCfg.Password.Length > 0;

        if (areCfgPropsSetted)
        {
            configurator.Host(rabbitMqCfg.Host, "/", host =>
            {
                host.Username(rabbitMqCfg.Username);
                host.Password(rabbitMqCfg.Password);
            });
            return;
        }

        configurator.Host(rabbitMqCfg.Host, "/", host =>
        {
            host.Username("guest");
            host.Password("guest");
        });
    }
}