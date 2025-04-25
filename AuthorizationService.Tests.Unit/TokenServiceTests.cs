using System.Collections;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Authentication.Configuration.Options;
using AuthorizationService.Common.Services;
using AuthorizationService.Data;
using AuthorizationService.Entities;
using AuthorizationService.Entities.Tokens;
using AuthorizationService.Services;
using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;

namespace AuthorizationService.Tests.Unit;

public class TokenServiceTests
{
    private readonly TokenService _sut;
    private readonly Mock<IdentityAppContext> _appContextMock;
    private readonly Mock<ICookieManager> _cookieManagerMock;
    private readonly Mock<ILogger<TokenService>> _loggerMock;
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly JwtOptions _jwtOptions;
    private readonly JwtOptions _optionsMock;

    public TokenServiceTests()
    {
        _appContextMock = new Mock<IdentityAppContext>();
        _cookieManagerMock = new Mock<ICookieManager>();
        _loggerMock = new Mock<ILogger<TokenService>>();
        _userManagerMock = new Mock<UserManager<User>>(
            Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);

        _jwtOptions = new JwtOptions
        {
            SecretKey = "very-long-and-secure-secret-key-with-aaaaaaaaaaaaaaaaaaaaaaaaaaaaaa-a-hms512-cond",
            AccessTokenExpiryMinutes = 30,
            RefreshTokenExpiryDays = 7,
            ShortRefreshTokenExpiryHours = 2,
            MaxRefreshTokensPerUser = 5,
            ValidIssuer = "test-issuer",
            ValidAudience = "test-audience"
        };

        _sut = new TokenService(_appContextMock.Object, _userManagerMock.Object, _cookieManagerMock.Object,
            Options.Create(_jwtOptions), _loggerMock.Object);
    }

    [Theory, AutoData]
    public async Task GenerateAccessTokenAsync_ValidUser_ShouldGenerateToken(User user)
    {
        // Arrange
        var claims = new List<Claim>
        {
            new("test-claim", "test-claim-value"),
            new("permission", "read")
        };
        var testRoles = new List<string> { "User", "Admin" };
        _userManagerMock.Setup((manager) => manager.GetClaimsAsync(user)).ReturnsAsync(claims);
        _userManagerMock.Setup((manager) => manager.GetRolesAsync(user)).ReturnsAsync(testRoles);

        //Act
        var result = await _sut.GenerateAccessTokenAsync(user);

        //Assert
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(result.TokenValue);

        //Verify standard props
        Assert.Equal(user.Id.ToString(), jwtToken.Subject);
        Assert.Equal(user.Email, jwtToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.Email).Value);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = _jwtOptions.ValidIssuer,
            ValidAudience = _jwtOptions.ValidAudience,
            ValidateLifetime = true,
        };

        var claimsVerified =
            tokenHandler.ValidateToken(result.TokenValue, validationParameters, out _);

        //Verify role claims
        var roleClaims = claimsVerified.FindAll(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
        Assert.Equal(testRoles.Count, roleClaims.Count);
        Assert.Equal(testRoles, roleClaims);

        //Verify custom claims
        Assert.Contains(claimsVerified.Claims, c => c is { Type: "test-claim", Value: "test-claim-value" });
        Assert.Contains(claimsVerified.Claims, c => c is { Type: "permission", Value: "read" });
    }

    [Theory, AutoData]
    public async Task GenerateAccessTokenAsync_NotValidHms512LengthSecretKey_Exception(User user)
    {
        // Arrange
        var changedOptions = _jwtOptions;
        changedOptions.SecretKey = "321321323terterre-t-ER-T_R_t-et-r_T-re-w4-3--r-e-t__$_";
        var claims = new List<Claim>
        {
            new("test-claim", "test-claim-value"),
            new("permission", "read")
        };
        var testRoles = new List<string> { "User", "Admin" };
        _userManagerMock.Setup((manager) => manager.GetClaimsAsync(user)).ReturnsAsync(claims);
        _userManagerMock.Setup((manager) => manager.GetRolesAsync(user)).ReturnsAsync(testRoles);

        //Act & Assert
        await Assert.ThrowsAsync<SecurityTokenException>(() => _sut.GenerateAccessTokenAsync(user));
    }

    [Theory, AutoData]
    public Task GenerateRefreshToken_WithRememberUser_CreatesLongLivedToken(string deviceId, string deviceName,
        string ipAddress)
    {
        // Arrange
        var rememberUser = true;

        // Act
        var resultToken = _sut.GenerateRefreshToken(deviceId, deviceName, ipAddress, rememberUser);

        // Assert
        Assert.NotNull(resultToken);
        Assert.Equal(64, Convert.FromBase64String(resultToken.TokenValue).Length);
        Assert.Equal(deviceId, resultToken.DeviceId);
        Assert.Equal(deviceName, resultToken.DeviceName);
        Assert.Equal(ipAddress, resultToken.IpAddress);
        Assert.True(resultToken.Expires >=
                    DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenExpiryDays) - TimeSpan.FromSeconds(30));
        return Task.CompletedTask;
    }

    [Theory, AutoData]
    public Task GenerateRefreshToken_WithoutRememberUser_CreatesShortLivedToken(string deviceId, string deviceName,
        string ipAddress)
    {
        // Arrange
        var rememberUser = false;

        // Act
        var resultToken = _sut.GenerateRefreshToken(deviceId, deviceName, ipAddress, rememberUser);

        // Assert
        Assert.NotNull(resultToken);
        Assert.Equal(64, Convert.FromBase64String(resultToken.TokenValue).Length);
        Assert.Equal(deviceId, resultToken.DeviceId);
        Assert.Equal(deviceName, resultToken.DeviceName);
        Assert.Equal(ipAddress, resultToken.IpAddress);
        Assert.True(resultToken.Expires <=
                    DateTime.UtcNow.AddHours(_jwtOptions.ShortRefreshTokenExpiryHours));
        return Task.CompletedTask;
    }

    [Theory, AutoData]
    public void ValidateRefreshTokenAsync_ValidToken_ReturnsTrue(User user, string deviceId, string deviceName,
        string ipAddress)
    {
        // Arrange
        var refreshToken = _sut.GenerateRefreshToken(deviceId, deviceName, ipAddress, false);
        user.RefreshTokens.Add(refreshToken);

        // Act
        var isResultTokenValid = _sut.ValidateRefreshTokenAsync(user, refreshToken.TokenValue);

        // Assert
        Assert.True(isResultTokenValid);
    }

    [Theory, AutoData]
    public void ValidateRefreshTokenAsync_NotValidToken_ReturnsFalse(User user, string deviceId, string deviceName,
        string ipAddress)
    {
        // Arrange
        var refreshToken = _sut.GenerateRefreshToken(deviceId, deviceName, ipAddress, false);
        refreshToken.Revoked = DateTime.UtcNow;
        user.RefreshTokens.Add(refreshToken);

        // Act
        var isResultTokenValid = _sut.ValidateRefreshTokenAsync(user, refreshToken.TokenValue);

        // Assert
        Assert.False(isResultTokenValid);
    }

    [Theory, AutoData]
    public async Task RevokeRefreshTokenAsync_ActiveToken_RevokesSuccessfully(User user, string deviceId,
        string deviceName,
        string ipAddress)
    {
        // Arrange
        var refreshToken = _sut.GenerateRefreshToken(deviceId, deviceName, ipAddress, true);
        user.RefreshTokens.Add(refreshToken);

        // Act
        await _sut.RevokeRefreshTokenAsync(user, refreshToken.TokenValue);

        // Assert
        Assert.NotNull(refreshToken.TokenValue);
        _appContextMock.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);
    }

    [Theory, AutoData]
    public async Task RevokeRefreshTokenAsync_AlreadyRevoked_SkipRevoking(User user, string deviceId,
        string deviceName,
        string ipAddress)
    {
        // Arrange
        var refreshToken = _sut.GenerateRefreshToken(deviceId, deviceName, ipAddress, true);
        refreshToken.Revoked = DateTime.UtcNow;
        user.RefreshTokens.Add(refreshToken);

        // Act
        await _sut.RevokeRefreshTokenAsync(user, refreshToken.TokenValue);

        // Assert
        Assert.NotNull(refreshToken.TokenValue);
        _appContextMock.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Never);
    }

    [Theory, AutoData]
    public async Task RemoveExpiredRefreshTokensAsync_ExpiredTokensExist_RemoveThemAll(User user)
    {
        // Arrange
        var refreshTokens = new List<RefreshToken>
        {
            _sut.GenerateRefreshToken("device-321", "device-name-321", "213.23.421.1", false),
            _sut.GenerateRefreshToken("device-322", "device-name-322", "213.23.421.2", false),
            _sut.GenerateRefreshToken("device-323", "device-name-323", "213.23.421.3", false),
        };

        refreshTokens[0].Revoked = DateTime.UtcNow;
        user.RefreshTokens.AddRange(refreshTokens);

        // Act
        var expiredTokensNum = await _sut.RemoveExpiredRefreshTokensAsync(user);

        // Assert
        Assert.Equal(2, user.RefreshTokens.Count);
        Assert.Equal(1, expiredTokensNum);
        _userManagerMock.Verify(x => x.UpdateAsync(user), Times.Once);
    }

    [Theory, AutoData]
    public async Task RemoveExpiredRefreshTokensAsync_ExpiredTokensNotExist_ReturnZero(User user)
    {
        // Arrange
        var refreshTokens = new List<RefreshToken>
        {
            _sut.GenerateRefreshToken("device-321", "device-name-321", "213.23.421.1", false),
            _sut.GenerateRefreshToken("device-322", "device-name-322", "213.23.421.2", false),
            _sut.GenerateRefreshToken("device-323", "device-name-323", "213.23.421.3", false),
        };

        user.RefreshTokens.AddRange(refreshTokens);

        // Act
        var expiredTokensNum = await _sut.RemoveExpiredRefreshTokensAsync(user);

        // Assert
        Assert.Equal(3, user.RefreshTokens.Count);
        Assert.Equal(0, expiredTokensNum);
        _userManagerMock.Verify(x => x.UpdateAsync(user), Times.Never);
    }

    [Theory, AutoData]
    public async Task StoreRefreshTokenAsync_EnoughTokenPositions_AddsToken(User user)
    {
        // Arrange
        _jwtOptions.MaxRefreshTokensPerUser = 4;
        var refreshTokens = new List<RefreshToken>
        {
            _sut.GenerateRefreshToken("device-321", "device-name-321", "213.23.421.1", false),
            _sut.GenerateRefreshToken("device-322", "device-name-322", "213.23.421.2", false),
            _sut.GenerateRefreshToken("device-323", "device-name-323", "213.23.421.3", false),
        };

        var refreshTokenToAdd = _sut.GenerateRefreshToken("device-324", "device-name-324", "213.23.421.4", false);

        user.RefreshTokens.AddRange(refreshTokens);

        // Act
        await _sut.StoreRefreshTokenAsync(user, refreshTokenToAdd);

        // Assert
        Assert.Equal(4, user.RefreshTokens.Count);
        Assert.Contains(refreshTokenToAdd, user.RefreshTokens);
        _userManagerMock.Verify(x => x.UpdateAsync(user), Times.Once);
    }

    [Theory, AutoData]
    public async Task StoreRefreshTokenAsync_NotEnoughTokenPositions_RemoveOldTokenAndAddNew(User user)
    {
        _jwtOptions.MaxRefreshTokensPerUser = 3;
        var refreshTokens = new List<RefreshToken>
        {
            _sut.GenerateRefreshToken("device-321", "device-name-321", "213.23.421.1", false),
            _sut.GenerateRefreshToken("device-322", "device-name-322", "213.23.421.2", false),
            _sut.GenerateRefreshToken("device-323", "device-name-323", "213.23.421.3", false),
        };

        var oldestToken = refreshTokens.MinBy(t => t.CreatedAt);
        var refreshTokenToAdd = _sut.GenerateRefreshToken("device-324", "device-name-324", "213.23.421.4", false);

        user.RefreshTokens.AddRange(refreshTokens);

        // Act
        await _sut.StoreRefreshTokenAsync(user, refreshTokenToAdd);

        // Assert
        Assert.Equal(_jwtOptions.MaxRefreshTokensPerUser, user.RefreshTokens.Count);
        Assert.DoesNotContain(oldestToken, user.RefreshTokens);
        Assert.Contains(refreshTokenToAdd, user.RefreshTokens);
        _userManagerMock.Verify(x => x.UpdateAsync(user), Times.Once);
    }

    [Fact]
    public void GetPrincipalFromExpiredToken_ExpiredToken_ReturnsPrincipal()
    {
        // Arrange
        var testToken = GenerateTestToken(DateTime.UtcNow.AddMinutes(-1));

        // Act
        var principal = _sut.GetPrincipalFromExpiredToken(testToken);

        // Assert
        Assert.NotNull(principal);
    }

    [Fact]
    public void GetPrincipalFromExpiredToken_ValidNotExpiredToken_ThrowsException()
    {
        // Arrange
        var testToken = GenerateTestToken(DateTime.UtcNow.AddHours(1));

        // Act
        var result = _sut.GetPrincipalFromExpiredToken(testToken);

        // Assert
        Assert.Null(result);
        _loggerMock.Verify(l => l.Log(LogLevel.Warning, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()));
    }

    [Fact]
    public void GetPrincipalFromExpiredToken_InvalidAlgorithm_ThrowsException()
    {
        // Arrange
        var testToken = GenerateTestToken(DateTime.UtcNow, SecurityAlgorithms.HmacSha256);

        // Act
        var result = _sut.GetPrincipalFromExpiredToken(testToken);

        Assert.Null(result);
        _loggerMock.Verify(l => l.Log(LogLevel.Warning, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()));
    }

    private string GenerateTestToken(DateTime expiration, string securityAlgorithm = SecurityAlgorithms.HmacSha512)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
        var credentials = new SigningCredentials(key, securityAlgorithm);

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.ValidIssuer,
            audience: _jwtOptions.ValidAudience,
            expires: expiration,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}