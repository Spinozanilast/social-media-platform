using Amazon.S3;
using AwsConfigurators;

namespace ProfileService.Data;

public static class S3StoreInjector
{
    public static void AddS3Client(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IAmazonS3>(_ =>
        {
            var configurator = new AwsS3ServiceConfigurator(configuration);
            var credentials = configurator.AwsCredentials;
            var config = configurator.GetServiceConfig();
            return new AmazonS3Client(credentials, (AmazonS3Config)config);
        });
    }
}