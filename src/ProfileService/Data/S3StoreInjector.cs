using Amazon.S3;
using AwsConfigurators;

namespace ProfileService.Data;

public static class S3StoreInjector
{
    private const string ServiceUrl = "https://s3.yandexcloud.net";

    public static void AddS3Client(this IServiceCollection services)
    {
        services.AddSingleton<IAmazonS3>(_ =>
        {
            var configurator = new AwsS3ServiceConfigurator();
            var credentials = configurator.AwsCredentials;
            var config = configurator.GetServiceConfig(serviceUrl: ServiceUrl);
            return new AmazonS3Client(credentials, (AmazonS3Config)config);
        });
    }
}