using Amazon.S3;
using AwsConfigurators;

namespace IdentityService.Data;

public static class S3StoreInjector
{
    private static readonly string ServiceUrl = "https://s3.yandexcloud.net";

    public static void AddS3Client(this IServiceCollection services)
    {
        services.AddSingleton<IAmazonS3>(opt =>
        {
            var configurator = new AwsS3ServiceConfigurator();
            var credentials = configurator.AwsCredentials;
            var config = configurator.GetServiceConfig(serviceUrl: ServiceUrl);
            return new AmazonS3Client(credentials, (AmazonS3Config)config);
        });
    }
}