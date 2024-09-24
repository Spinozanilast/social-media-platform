using Amazon.Runtime;
using Amazon.S3;

namespace AwsConfigurators;

public interface IAwsServiceConfigurator
{
    AWSCredentials AwsCredentials { get; set; }
    IClientConfig GetServiceConfig(string serviceUrl);
}