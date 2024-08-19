using Authentication.Configuration.Configurations;
using Authentication.Extensions.Configurations;

namespace Authentication.Extensions.Tests;

public class Tests
{
    [Test]
    public void Test_ConfigAttributesAreCorrect_WhenUserSecretsExists()
    {
        //Arrange 
        var jwtConfig = ConfigurationsManager.GetInstance().TokenConfiguration;

        //Act
        var isStringExists = !string.IsNullOrEmpty(jwtConfig.SecretKey);
        var isCorrectNumber = jwtConfig.ExpiryDays > 0;

        //Assert
        Assert.IsTrue(isStringExists);
        Assert.IsTrue(isCorrectNumber);
    }
}