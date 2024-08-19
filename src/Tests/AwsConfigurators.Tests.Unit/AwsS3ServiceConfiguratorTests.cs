namespace AwsConfigurators.Tests.Unit;

public class AwsS3ServiceConfiguratorTests
{
    [Fact]
    public void Test_AwsConfigurationIsCorrect_WhenItExists()
    {
        //Arrange
        var configurator = new AwsS3ServiceConfigurator();
        var testUrl = "https://platform-cloud.com/";

        //Act
        var credentialsResult = configurator.AwsCredentials;
        var configResult = configurator.GetServiceConfig(testUrl);

        //Assert
        Assert.NotNull(credentialsResult);
        Assert.NotNull(configResult);
        Assert.NotEmpty(credentialsResult.GetCredentials().SecretKey);
        Assert.NotEmpty(credentialsResult.GetCredentials().AccessKey);
        Assert.Equal(testUrl, configResult.ServiceURL);
    }
}