using Authentication.Extensions.Configurations;
using Microsoft.Extensions.Configuration;

namespace Authentication.Configuration.Configurations;

public class ConfigurationsManager
{
    public TokenConfiguration TokenConfiguration { get; set; }

    private ConfigurationsManager()
    {
        InitTokenConfiguration();
    }

    private static ConfigurationsManager? _instance;

    public static ConfigurationsManager GetInstance()
    {
        if (_instance is null)
        {
            _instance = new ConfigurationsManager();
        }

        return _instance;
    }

    private void InitTokenConfiguration()
    {
        var configRoot = new ConfigurationBuilder().AddUserSecrets<ConfigurationsManager>().Build();
        
        var secretKey = configRoot["jwt:SecretKey"];
        var expiryDays = configRoot["jwt:ExpiryDays"];
        
        if (string.IsNullOrEmpty(secretKey) || !int.TryParse(expiryDays, out var expiryDaysParsed))
        {
            throw new Exception("User Secrets were not configured");
        }

        TokenConfiguration = new TokenConfiguration()
        {
            ExpiryDays = expiryDaysParsed,
            SecretKey = secretKey
        };
    }
}