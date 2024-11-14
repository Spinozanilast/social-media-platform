namespace Shared.Infrastructure.Configurations;

public class RabbitMqConfiguration
{
    public const string SectionName = "rabbitmq";

    public string Host { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}