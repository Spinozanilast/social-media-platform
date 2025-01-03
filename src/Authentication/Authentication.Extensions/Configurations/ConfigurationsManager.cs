using Microsoft.Extensions.Configuration;

namespace Authentication.Configuration.Configurations;

public class ConfigurationsManager
{
    public TokenConfiguration TokenConfiguration { get; private set; }

    private ConfigurationsManager()
    {
        InitTokenConfiguration();
    }

    private static ConfigurationsManager? _instance;

    public static ConfigurationsManager GetInstance()
    {
        return _instance ??= new ConfigurationsManager();
    }

    private void InitTokenConfiguration()
    {
        var configRoot = new ConfigurationBuilder().AddUserSecrets<ConfigurationsManager>().AddEnvironmentVariables()
            .Build();

        var secretKey = configRoot["JWT:SecretKey"];
        var expiryDays = configRoot["JWT:ExpiryDays"];

        if (string.IsNullOrEmpty(secretKey) || !int.TryParse(expiryDays, out var expiryDaysParsed))
        {
            throw new Exception("User Secrets were not configured");
        }

        TokenConfiguration = new TokenConfiguration
        {
            ExpiryDays = expiryDaysParsed,
            SecretKey = secretKey
        };
    }
}