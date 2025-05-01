using Authentication.Configuration;
using Authentication.Configuration.Options;
using AuthorizationService.Entities.Tokens;
using AuthorizationService.Services;
using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;

namespace AuthorizationService.Tests.Unit;

public class CookieManagerTests
{
    private readonly CookieManager _sut;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessor;
    private readonly JwtOptions _jwtOptions;

    public CookieManagerTests()
    {
        _httpContextAccessor = new Mock<IHttpContextAccessor>();
        _jwtOptions = SharedTestData.JwtOptions;
        _sut = new CookieManager(_httpContextAccessor.Object, Options.Create(_jwtOptions));
    }

    [Theory, AutoData]
    public void SetAuthCookies_ShouldSetCookies(Token accessToken, RefreshToken refreshToken)
    {
        // Arrange
        var responseCookiesMock = new Mock<IResponseCookies>();
        var httpContextMock = new Mock<HttpContext>();
        var httpResponseMock = new Mock<HttpResponse>();

        httpContextMock.SetupGet(c => c.Response).Returns(httpResponseMock.Object);
        httpResponseMock.SetupGet(c => c.Cookies).Returns(responseCookiesMock.Object);
        _httpContextAccessor.SetupGet(c => c.HttpContext).Returns(httpContextMock.Object);

        // Act 
        _sut.SetAuthCookies(accessToken, refreshToken);

        // Assert
        VerifyResponseCookiesMockAppendedCookie(responseCookiesMock, _jwtOptions.CookieNames[AuthCookieTypes.JwtCookie],
            accessToken.TokenValue);
        VerifyResponseCookiesMockAppendedCookie(responseCookiesMock,
            _jwtOptions.CookieNames[AuthCookieTypes.RefreshCookie],
            refreshToken.TokenValue);
    }

    [Fact]
    public void ClearAuthCookies_ShouldClearCookies()
    {
        // Arrange
        var responseCookiesMock = new Mock<IResponseCookies>();
        var httpContextMock = new Mock<HttpContext>();
        var httpResponseMock = new Mock<HttpResponse>();

        httpContextMock.SetupGet(c => c.Response).Returns(httpResponseMock.Object);
        httpResponseMock.SetupGet(c => c.Cookies).Returns(responseCookiesMock.Object);
        _httpContextAccessor.SetupGet(c => c.HttpContext).Returns(httpContextMock.Object);

        // Act 
        _sut.ClearAuthCookies();

        // Assert
        responseCookiesMock.Verify(c => c.Delete(_jwtOptions.CookieNames[AuthCookieTypes.JwtCookie],
            It.Is<CookieOptions>(o => o.Path == "/")), Times.Once);
        responseCookiesMock.Verify(c => c.Delete(_jwtOptions.CookieNames[AuthCookieTypes.RefreshCookie],
            It.Is<CookieOptions>(o => o.Path == "/")), Times.Once);
    }

    [Theory, AutoData]
    public void GetAuthCookies_ValidContext_ShouldReturnCookiesValues(string accessToken, string refreshToken)
    {
        // Arrange
        var requestCookies = new Mock<IRequestCookieCollection>();
        var httpContextMock = new Mock<HttpContext>();
        var httpRequestMock = new Mock<HttpRequest>();

        requestCookies.Setup(c => c[_jwtOptions.CookieNames[AuthCookieTypes.JwtCookie]]).Returns(accessToken);
        requestCookies.Setup(c => c[_jwtOptions.CookieNames[AuthCookieTypes.RefreshCookie]]).Returns(refreshToken);
        httpRequestMock.SetupGet(r => r.Cookies).Returns(requestCookies.Object);
        httpContextMock.SetupGet(c => c.Request).Returns(httpRequestMock.Object);
        _httpContextAccessor.SetupGet(c => c.HttpContext).Returns(httpContextMock.Object);


        // Act
        var tokens = _sut.GetAuthCookies();

        // Assert
        Assert.Equal(accessToken, tokens.AccessToken);
        Assert.Equal(refreshToken, tokens.RefreshToken);
    }

    [Fact]
    public void GetAuthCookies_NullContext_ShouldReturnCookiesValues()
    {
        // Arrange
        _httpContextAccessor.SetupGet(c => c.HttpContext).Returns((HttpContext)null);

        // Act
        var tokens = _sut.GetAuthCookies();

        // Assert
        Assert.Null(tokens.AccessToken);
        Assert.Null(tokens.RefreshToken);
    }

    private void VerifyResponseCookiesMockAppendedCookie(Mock<IResponseCookies> responseCookiesMock, string cookieName,
        string cookieValue)
    {
        responseCookiesMock.Verify(c => c.Append(cookieName,
            cookieValue, It.Is<CookieOptions>(o =>
                o.HttpOnly && o.Secure && o.SameSite == SameSiteMode.Strict && o.Secure)), Times.Once);
    }
}