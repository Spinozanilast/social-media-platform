using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.S3;
using Microsoft.Extensions.Configuration;

namespace AwsConfigurators;

public class AwsS3ServiceConfigurator : IAwsServiceConfigurator
{
    public AWSCredentials AwsCredentials { get; set; }
    private readonly IConfiguration _configuration;

    public AwsS3ServiceConfigurator(IConfiguration configuration)
    {
        _configuration = configuration;
        InitCredentials();
    }

    private void InitCredentials()
    {
        var profileName = _configuration["AWS:ProfileName"];
        var accessKey = _configuration["AWS:AccessKey"];
        var secretKey = _configuration["AWS:SecretKey"];

        if ((string.IsNullOrEmpty(accessKey) || string.IsNullOrEmpty(secretKey)) && !string.IsNullOrEmpty(profileName))
        {
            var profileStoreChain = new CredentialProfileStoreChain();
            if (profileStoreChain.TryGetProfile(profileName, out var credentials))
            {
                AwsCredentials = new BasicAWSCredentials(credentials.Options.AccessKey, credentials.Options.SecretKey);
            }
            else
            {
                throw new Exception($"Failed to find the {profileName} profile");
            }
        }
        else
        {
            AwsCredentials = new BasicAWSCredentials(accessKey, secretKey);
        }
    }

    public IClientConfig GetServiceConfig()
    {
        var serviceUrl = _configuration["AWS:ServiceUrl"];
        return new AmazonS3Config { ServiceURL = serviceUrl };
    }
}