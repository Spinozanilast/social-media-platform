using Authentication.Configuration.Configurations;
using Xunit;

namespace Authentication.Extensions.Tests;

public class ConfigurationsManagerTests
{
    [Fact]
    public void Test_ConfigAttributesAreCorrect_WhenUserSecretsExists()
    {
        //Arrange 
        var jwtConfig = ConfigurationsManager.GetInstance().TokenConfiguration;

        //Act
        var isStringExists = !string.IsNullOrEmpty(jwtConfig.SecretKey);
        var isCorrectNumber = jwtConfig.ExpiryDays > 0;

        //Assert
        Assert.True(isStringExists);
        Assert.True(isCorrectNumber);
    }
}