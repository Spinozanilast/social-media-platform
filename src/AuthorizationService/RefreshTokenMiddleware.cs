using AuthorizationService.Common.Services;
using AuthorizationService.Data;
using AuthorizationService.Entities;
using AuthorizationService.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace AuthorizationService;

public class RefreshTokenMiddleware
{
    private readonly ILogger<RefreshTokenMiddleware> _logger;
    private readonly RequestDelegate _next;

    public RefreshTokenMiddleware(RequestDelegate next, ILogger<RefreshTokenMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(
        HttpContext context,
        ITokenService tokenService,
        ICookieManager cookieManager,
        IdentityAppContext db,
        UserManager<User> userManager)
    {
        var endpoint = context.GetEndpoint();

        if (endpoint is null || endpoint.Metadata.GetMetadata<IAllowAnonymous>() is not null)
        {
            await _next(context);
            return;
        }

        var (accessTokenValue, refreshTokenValue) = cookieManager.GetAuthCookies();

        if (!string.IsNullOrEmpty(accessTokenValue)
            || string.IsNullOrEmpty(refreshTokenValue) ||
            !tokenService.IsTokenExpired(accessTokenValue))
        {
            await _next(context);
            return;
        }

        var user = userManager
            .Users
            .SingleOrDefault(u => u.RefreshTokens.Any(t => t.TokenValue == refreshTokenValue));

        if (user is null)
        {
            await _next(context);
            return;
        }

        var refreshToken = user.RefreshTokens.Single(t => t.TokenValue == refreshTokenValue);

        if (!refreshToken.IsActive)
        {
            await _next(context);
            return;
        }

        refreshToken.Revoked = DateTime.UtcNow;

        var newRefreshToken = tokenService.GenerateRefreshToken(context.GetDeviceInfo(), refreshToken.IsLongActive);
        user.RefreshTokens.Add(newRefreshToken);
        var updateResult = await userManager.UpdateAsync(user);

        if (!updateResult.Succeeded)
        {
            _logger.LogError("Failed to update refresh token: {NewRefreshToken}", newRefreshToken);
            context.Response.StatusCode = 500;
            await _next(context);
        }

        var newAccessToken = await tokenService.GenerateAccessTokenAsync(user);
        context.User = tokenService.GetPrincipalFromToken(newAccessToken.TokenValue);
        cookieManager.SetAuthCookies(newAccessToken, newRefreshToken);

        await _next(context);
    }
}

public static class RefreshTokenMiddlewareExtensions
{
    public static IApplicationBuilder UseRefreshTokenMiddleware(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RefreshTokenMiddleware>();
    }
}