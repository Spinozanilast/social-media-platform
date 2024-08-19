using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.S3;

namespace AwsConfigurators;

public class AwsS3ServiceConfigurator : IAwsServiceConfigurator
{
    public AWSCredentials AwsCredentials { get; set; }

    public AwsS3ServiceConfigurator(string profileName = "default")
    {
        InitCredentials(profileName);
    }

    private void InitCredentials(string profileName)
    {
        var profileStoreChain = new CredentialProfileStoreChain();
        
        if (!profileStoreChain.TryGetProfile(profileName, out var credentials))
        {
            throw new Exception($"Failed to find the {profileName} profile");
        }

        var accessKey = credentials.Options.AccessKey;
        var secretKey = credentials.Options.SecretKey;

        AwsCredentials = new BasicAWSCredentials(accessKey, secretKey);
    }

    public IClientConfig GetServiceConfig(string serviceUrl)
    {
        return new AmazonS3Config()
        {
            ServiceURL = serviceUrl
        };
    }
}