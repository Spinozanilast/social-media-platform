using Amazon.Runtime;

namespace AwsConfigurators;

public interface IAwsServiceConfigurator
{
    AWSCredentials AwsCredentials { get; set; }
    IClientConfig GetServiceConfig();
}