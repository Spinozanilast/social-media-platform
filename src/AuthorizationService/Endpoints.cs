using System.Security.Claims;
using AuthorizationService.Common.Mappers;
using AuthorizationService.Common.Services;
using AuthorizationService.Contracts.Devices;
using AuthorizationService.Contracts.Login;
using AuthorizationService.Contracts.Register;
using AuthorizationService.Contracts.Users;
using AuthorizationService.Entities;
using AuthorizationService.Entities.OAuthInfos;
using AuthorizationService.Extensions;
using MassTransit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace AuthorizationService;

public static class Endpoints
{
    public static RouteGroupBuilder MapAuthEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost("/register",
                async Task<Results<Created<RegisterResponse>, BadRequest<RegisterErrorsResponse>>>
                (
                    [FromBody] RegisterRequest request,
                    [FromServices] ILogger<Program> logger,
                    [FromServices] IPublishEndpoint publishEndpoint,
                    [FromServices] UserManager<User> userManager) =>
                {
                    var user = new User
                    {
                        UserName = request.UserName, FirstName = request.FirstName, LastName = request.LastName,
                        Email = request.Email
                    };

                    var result = await userManager.CreateAsync(user, request.Password);

                    if (!result.Succeeded)
                    {
                        logger.LogWarning("Registration failed for {Email}: {Errors}", request.Email, result.Errors);
                        return TypedResults.BadRequest(result.ToDefaultErrorResponse());
                    }


                    await userManager.AddToRoleAsync(user, IdentityRoles.UserRole.Name ?? string.Empty);

                    logger.LogInformation("New user registered: {UserId}", user.Id);

                    try
                    {
                        var registeredUser = new UserRegistered(user.Id, null);
                        await publishEndpoint.Publish(registeredUser);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Failed to publish registered user to Profile service");
                    }

                    return TypedResults.Created($"/api/v1/auth/{user.Id}",
                        new RegisterResponse(user.Id, user.UserName));
                })
            .AllowAnonymous()
            .WithName("Register");

        group.MapPost("/login", async Task<Results<Ok<AuthResponse>, ProblemHttpResult>>
            (
                [FromBody] LoginRequest request,
                [FromServices] ILogger<Program> logger,
                [FromServices] IPublishEndpoint publishEndpoint,
                [FromServices] UserManager<User> userManager,
                [FromServices] ITokenService tokenService,
                [FromServices] ICookieManager cookieManager,
                HttpContext httpContext) =>
            {
                var user = await userManager.FindByEmailAsync(request.Email);

                if (user is null || !await userManager.CheckPasswordAsync(user, request.Password))
                {
                    logger.LogWarning("Failed login attempt for {Email}", request.Email);
                    return TypedResults.Problem(title: "Invalid Credentials", detail: "Email or password is incorrect",
                        statusCode: StatusCodes.Status401Unauthorized);
                }

                if (await userManager.IsLockedOutAsync(user))
                {
                    logger.LogWarning("Locked out account attempt: {Email}", request.Email);
                    return TypedResults.Problem(title: "Account locked", detail: "Too many failed login attempts",
                        statusCode: StatusCodes.Status401Unauthorized);
                }

                var deviceId = httpContext.Request.Headers["X-Device-Id"].ToString();
                var deviceName = httpContext.Request.Headers["X-Device-Name"].ToString();
                var ipAddress = httpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

                var accessToken = await tokenService.GenerateAccessTokenAsync(user);
                var refreshToken =
                    tokenService.GenerateRefreshToken(deviceId, deviceName, ipAddress, request.RememberMe);

                cookieManager.SetAuthCookies(accessToken, refreshToken);

                await tokenService.StoreRefreshTokenAsync(user, refreshToken);

                logger.LogInformation("User logged in: {UserId}", user.Id);

                return TypedResults.Ok(new AuthResponse(
                    UserId: user.Id,
                    UserName: user.UserName ?? string.Empty,
                    Email: user.Email ?? string.Empty,
                    Roles: await userManager.GetRolesAsync(user),
                    AccessTokenExpiry: accessToken.Expires,
                    GithubInfo: user.GithubInfo
                ));
            })
            .AllowAnonymous()
            .WithName("Login");

        group.MapGet("/github/login",
                ChallengeHttpResult () =>
                    TypedResults.Challenge(
                        new AuthenticationProperties
                            { RedirectUri = "/api/v1/auth/signin-github" },
                        ["Github"]))
            .AllowAnonymous()
            .WithName("GithubLogin");

        group.MapGet("/signin-github", async Task<Results<RedirectHttpResult, UnauthorizedHttpResult>> (
                [FromServices] ITokenService tokenService,
                [FromServices] ICookieManager cookieManager,
                [FromServices] UserManager<User> userManager,
                [FromServices] IConfiguration configuration,
                [FromServices] ILogger<Program> logger,
                [FromServices] IPublishEndpoint publishEndpoint,
                HttpContext context) =>
            {
                var authResult = await context.AuthenticateAsync("Github");
                if (!authResult.Succeeded) return TypedResults.Unauthorized();


                var principal = authResult.Principal;

                var githubId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
                var avatarUrl = principal.FindFirstValue("urn:github:avatar_url");
                var profileUrl = principal.FindFirstValue("urn:github:html_url");
                var username = principal.FindFirstValue(ClaimTypes.Name);
                var email = principal.FindFirstValue(ClaimTypes.Email)
                            ?? principal.FindFirstValue("urn:github:email");

                var user = string.IsNullOrEmpty(email)
                    ? await userManager.FindByNameAsync(username)
                    : await userManager.FindByEmailAsync(email);

                if (user is null)
                {
                    user = new User
                    {
                        UserName = username,
                        Email = email,
                        GithubInfo = new GithubInfo
                        {
                            GithubId = githubId,
                            GithubEmail = email,
                            GithubUsername = username,
                            ProfileUrl = profileUrl,
                            AvatarUrl = avatarUrl,
                        }
                    };

                    await userManager.CreateAsync(user);
                    await userManager.AddToRoleAsync(user, IdentityRoles.UserRole.Name!);

                    try
                    {
                        var registeredUser = new UserRegistered(user.Id, [profileUrl!]);
                        await publishEndpoint.Publish(registeredUser);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Failed to publish registered user to Profile service");
                    }
                }

                var deviceId = context.Request.Headers["X-Device-Id"].ToString();
                var deviceName = context.Request.Headers["X-Device-Name"].ToString();
                var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

                var accessToken = await tokenService.GenerateAccessTokenAsync(user);
                var refreshToken = tokenService.GenerateRefreshToken(deviceId, deviceName, ipAddress, true);

                cookieManager.SetAuthCookies(accessToken, refreshToken);
                await tokenService.StoreRefreshTokenAsync(user, refreshToken);

                return TypedResults.Redirect(configuration["Frontend:SuccessRedirectUrl"]);
            })
            .AllowAnonymous()
            .WithName("GitHubCallback");

        group.MapPost("/logout", async Task<Results<NoContent, UnauthorizedHttpResult>> (
                [FromServices] ILogger<Program> logger,
                [FromServices] IPublishEndpoint publishEndpoint,
                [FromServices] UserManager<User> userManager,
                [FromServices] ITokenService tokenService,
                [FromServices] ICookieManager cookieManager,
                ClaimsPrincipal claimsPrincipal) =>
            {
                var userId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId is null)
                {
                    logger.LogWarning("Cannot logout user, user id was not found");
                    return TypedResults.Unauthorized();
                }

                var user = await userManager.FindByIdAsync(userId);
                var currentRefreshToken = cookieManager.GetAuthCookies().RefreshToken;

                if (user is not null && currentRefreshToken is not null)
                {
                    await tokenService.RevokeRefreshTokenAsync(user, currentRefreshToken);
                    logger.LogInformation("User logged out: {UserId}", userId);
                }

                cookieManager.ClearAuthCookies();

                return TypedResults.NoContent();
            })
            .RequireAuthorization()
            .WithName("Logout");

        group.MapPost("/me", async Task<Results<Ok, Ok<UserDto>, NotFound>> (
                [FromServices] ILogger<Program> logger,
                [FromServices] IPublishEndpoint publishEndpoint,
                [FromServices] UserManager<User> userManager,
                [FromServices] ITokenService tokenService,
                [FromServices] ICookieManager cookieManager,
                ClaimsPrincipal claimsPrincipal,
                [FromQuery] bool isUserResponse = false) =>
            {
                var currentUserId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);

                if (currentUserId is null)
                {
                    return TypedResults.NotFound();
                }

                var user = await userManager.FindByIdAsync(currentUserId);

                if (user is null)
                {
                    return TypedResults.NotFound();
                }

                return isUserResponse ? TypedResults.Ok(user.ToUserDto()) : TypedResults.Ok();
            })
            .RequireAuthorization()
            .WithName("GetCurrentUser");

        group.MapGet("{idOrUsername}", async Task<Results<Ok<UserDto>, NotFound>> (
                [FromRoute] string idOrUsername,
                [FromServices] UserManager<User> userManager) =>
            {
                var user = await userManager.FindUserBySlugAsync(idOrUsername);

                if (user is not null)
                {
                    return TypedResults.Ok(user.ToUserDto());
                }

                return TypedResults.NotFound();
            })
            .AllowAnonymous()
            .WithName("GetUser");

        group.MapPost("/refresh-token", async Task<Results<Ok<AuthResponse>, ProblemHttpResult>> (
                [FromServices] ILogger<Program> logger,
                [FromServices] ICookieManager cookieManager,
                [FromServices] ITokenService tokenService,
                [FromServices] UserManager<User> userManager,
                HttpContext httpContext) =>
            {
                var cookies = cookieManager.GetAuthCookies();
                var accessToken = cookies.AccessToken;
                var refreshToken = cookies.RefreshToken;

                if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
                {
                    return TypedResults.Problem(
                        title: "Missing tokens",
                        detail: "Both access and refresh tokens are required",
                        statusCode: StatusCodes.Status401Unauthorized
                    );
                }

                var principal = tokenService.GetPrincipalFromExpiredToken(accessToken);
                var userId = principal?.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                {
                    return TypedResults.Problem(
                        title: "Invalid token",
                        detail: "Unable to extract user ID from access token",
                        statusCode: StatusCodes.Status401Unauthorized
                    );
                }

                var user = await userManager.FindByIdAsync(userId);
                if (user is null || !tokenService.ValidateRefreshTokenAsync(user, refreshToken))
                {
                    logger.LogWarning("Invalid refresh token attempt for user {UserId}", userId);
                    return TypedResults.Problem(
                        title: "Invalid token",
                        detail: "Refresh token validation failed",
                        statusCode: StatusCodes.Status401Unauthorized
                    );
                }

                var deviceId = principal?.FindFirst("deviceId")?.Value;
                var storedToken =
                    user.RefreshTokens.FirstOrDefault(rt => rt.TokenValue == refreshToken && rt.DeviceId == deviceId);

                if (storedToken is null || !storedToken.IsActive)
                {
                    logger.LogWarning("Refresh token for that device was not found or inactive for user {UserId}",
                        userId);
                    return TypedResults.Problem(
                        title: "Device not found",
                        detail: "Refresh token not exists or inactive",
                        statusCode: StatusCodes.Status401Unauthorized
                    );
                }

                var newAccessToken = await tokenService.GenerateAccessTokenAsync(user);

                var clientIpAddress = httpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
                var newRefreshToken =
                    tokenService.GenerateRefreshToken(storedToken.DeviceId, storedToken.DeviceName, clientIpAddress,
                        true);

                await tokenService.StoreRefreshTokenAsync(user, newRefreshToken);
                await tokenService.RevokeRefreshTokenAsync(user, refreshToken);

                cookieManager.SetAuthCookies(newAccessToken, newRefreshToken);

                return TypedResults.Ok(new AuthResponse(
                    user.Id, user.UserName ?? string.Empty, user.Email ?? string.Empty,
                    await userManager.GetRolesAsync(user),
                    newAccessToken.Expires, null));
            })
            .AllowAnonymous()
            .WithName("RefreshToken");

        group.MapGet("/devices", async Task<Results<Ok<IEnumerable<DeviceInfoResponse>>, UnauthorizedHttpResult>> (
                [FromServices] UserManager<User> userManager,
                ClaimsPrincipal userClaimsPrincipal) =>
            {
                var user = await userManager.GetUserAsync(userClaimsPrincipal);
                if (user is null) return TypedResults.Unauthorized();

                var activeSessions = user.RefreshTokens
                    .Where(rt => rt.IsActive)
                    .Select(rt => new DeviceInfoResponse(
                        rt.DeviceId,
                        rt.DeviceName,
                        rt.IpAddress,
                        rt.CreatedAt,
                        rt.Expires));

                return TypedResults.Ok(activeSessions);
            })
            .RequireAuthorization()
            .WithName("GetDevices");

        group.MapPost("/devices/revoke", async Task<Results<NoContent, ProblemHttpResult, UnauthorizedHttpResult>> (
                [FromBody] RevokeDeviceRequest request,
                [FromServices] ICookieManager cookieManager,
                [FromServices] UserManager<User> userManager,
                [FromServices] ITokenService tokenService,
                ClaimsPrincipal userClaimsPrincipal,
                HttpContext httpContext) =>
            {
                var user = await userManager.GetUserAsync(userClaimsPrincipal);
                if (user == null) return TypedResults.Unauthorized();

                var tokenToRevoke = user.RefreshTokens
                    .FirstOrDefault(rt => rt.DeviceId == request.DeviceId && rt.IsActive);

                if (tokenToRevoke == null)
                {
                    return TypedResults.Problem(
                        title: "Device not found",
                        detail: "No active session found for the specified device",
                        statusCode: StatusCodes.Status404NotFound
                    );
                }

                await tokenService.RevokeRefreshTokenAsync(user, tokenToRevoke.TokenValue);

                var currentDeviceId = httpContext.Request.Headers["X-Device-Id"].ToString();

                if (tokenToRevoke.DeviceId == currentDeviceId)
                {
                    cookieManager.ClearAuthCookies();
                }

                return TypedResults.NoContent();
            })
            .RequireAuthorization()
            .WithName("RevokeDevice");

        group.MapOpenApi();

        return group;
    }
}