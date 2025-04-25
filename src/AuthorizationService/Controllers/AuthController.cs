using System.Security.Claims;
using Asp.Versioning;
using AuthorizationService.Common.Services;
using AuthorizationService.Contracts.Devices;
using AuthorizationService.Contracts.Login;
using AuthorizationService.Contracts.Register;
using AuthorizationService.Entities;
using AuthorizationService.Entities.Tokens;
using AuthorizationService.Common.Mappers;
using AuthorizationService.Helpers.Extensions;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace AuthorizationService.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/auth")]
[ApiVersion("1.0")]
public class AuthController : ControllerBase
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ITokenService _tokenService;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<AuthController> _logger;
    private readonly ICookieManager _cookieManager;

    public AuthController(IPublishEndpoint publishEndpoint,
        ILogger<AuthController> logger, UserManager<User> userManager, ITokenService tokenService,
        ICookieManager cookieManager)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
        _userManager = userManager;
        _tokenService = tokenService;
        _cookieManager = cookieManager;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var user = new User
        {
            UserName = request.UserName, FirstName = request.FirstName, LastName = request.LastName,
            Email = request.Email
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            _logger.LogWarning("Registration failed for {Email}: {Errors}", request.Email, result.Errors);
            return BadRequest(result.ToDefaultErrorResponse());
        }


        await _userManager.AddToRoleAsync(user, IdentityRoles.UserRole.Name ?? string.Empty);

        _logger.LogInformation("New user registered: {UserId}", user.Id);

        try
        {
            var registeredUser = new UserRegistered(user.Id);
            await _publishEndpoint.Publish(registeredUser);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish registered user to Profile service");
        }

        return CreatedAtAction(nameof(GetUser), new { idOrUsername = user.Id.ToString() },
            new RegisterResponse(user.Id, user.UserName));
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            _logger.LogWarning("Failed login attempt for {Email}", request.Email);
            return Unauthorized(new ProblemDetails
            {
                Title = "Invalid Credentials",
                Detail = "Email or password is incorrect",
            });
        }

        if (await _userManager.IsLockedOutAsync(user))
        {
            _logger.LogWarning("Locked out account attempt: {Email}", request.Email);
            return Unauthorized(new ProblemDetails
            {
                Title = "Account locked",
                Detail = "Too many failed login attempts"
            });
        }

        var deviceInfo = GetDeviceInfo();
        var tokenPair =
            await GenerateSetTokensAsync(user, deviceInfo.deviceId, deviceInfo.deviceName, deviceInfo.ipAddress,
                request.RememberMe);

        _logger.LogInformation("User logged in: {UserId}", user.Id);

        return Ok(new AuthResponse(
            user.Id,
            user.UserName ?? string.Empty,
            user.Email ?? string.Empty,
            await _userManager.GetRolesAsync(user),
            tokenPair.AccessToken.Expires
        ));
    }

    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Logout()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null)
        {
            _logger.LogWarning("Cannot logout user, user id was not found");
            return Unauthorized();
        }

        var user = await _userManager.FindByIdAsync(userId);
        var currentRefreshToken = _cookieManager.GetAuthCookies().RefreshToken;

        if (user is not null && currentRefreshToken is not null)
        {
            await _tokenService.RevokeRefreshTokenAsync(user, currentRefreshToken);
            _logger.LogInformation("User logged out: {UserId}", userId);
        }

        _cookieManager.ClearAuthCookies();

        return NoContent();
    }

    [HttpPost("me")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCurrentUser([FromQuery] bool isUserResponse = false)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (currentUserId is null)
        {
            return NotFound();
        }

        var user = await _userManager.FindByIdAsync(currentUserId);

        if (user is null)
        {
            return NotFound();
        }

        return isUserResponse ? Ok(user.ToUserDto()) : Ok();
    }

    [HttpGet("{idOrUsername}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUser([FromRoute] string idOrUsername)
    {
        var user = await _userManager.FindUserBySlugAsync(idOrUsername);

        if (user is not null)
        {
            return Ok(user.ToUserDto());
        }

        return NotFound();
    }

    [AllowAnonymous]
    [HttpPost("refresh-token")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshToken()
    {
        var cookies = _cookieManager.GetAuthCookies();
        var accessToken = cookies.AccessToken;
        var refreshToken = cookies.RefreshToken;

        if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
        {
            return Unauthorized(new ProblemDetails
            {
                Title = "Missing tokens",
                Detail = "Both access and refresh tokens are required"
            });
        }

        var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
        var userId = principal?.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized(new ProblemDetails
            {
                Title = "Invalid token",
                Detail = "Unable to extract user ID from access token"
            });
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user is null || !_tokenService.ValidateRefreshTokenAsync(user, refreshToken))
        {
            _logger.LogWarning("Invalid refresh token attempt for user {UserId}", userId);
            return Unauthorized(new ProblemDetails
            {
                Title = "Invalid token",
                Detail = "Refresh token validation failed"
            });
        }

        var deviceId = principal?.FindFirst("deviceId")?.Value;
        var storedToken =
            user.RefreshTokens.FirstOrDefault(rt => rt.TokenValue == refreshToken && rt.DeviceId == deviceId);

        if (storedToken is null || !storedToken.IsActive)
        {
            _logger.LogWarning("Refresh token for that device was not found or inactive for user {UserId}", userId);
            return Unauthorized(new ProblemDetails
            {
                Title = "Device not found",
                Detail = "Refresh token not exists or inactive"
            });
        }

        var newAccessToken = await _tokenService.GenerateAccessTokenAsync(user);
        var newRefreshToken =
            _tokenService.GenerateRefreshToken(storedToken.DeviceId, storedToken.DeviceName, GetClientIpAddress(),
                true);

        await _tokenService.StoreRefreshTokenAsync(user, newRefreshToken);
        await _tokenService.RevokeRefreshTokenAsync(user, refreshToken);

        _cookieManager.SetAuthCookies(newAccessToken, newRefreshToken);

        return Ok(new AuthResponse(
            user.Id, user.UserName ?? string.Empty, user.Email ?? string.Empty, await _userManager.GetRolesAsync(user),
            newAccessToken.Expires));
    }

    [Authorize]
    [HttpGet("devices")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetActiveSessions()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is null) return Unauthorized();

        var activeSessions = user.RefreshTokens
            .Where(rt => rt.IsActive)
            .Select(rt => new DeviceInfoResponse(
                rt.DeviceId,
                rt.DeviceName,
                rt.IpAddress,
                rt.CreatedAt,
                rt.Expires));

        return Ok(activeSessions);
    }

    [Authorize]
    [HttpPost("devices/revoke")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RevokeDevice([FromBody] RevokeDeviceRequest request)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var tokenToRevoke = user.RefreshTokens
            .FirstOrDefault(rt => rt.DeviceId == request.DeviceId && rt.IsActive);

        if (tokenToRevoke == null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Device not found",
                Detail = "No active session found for the specified device"
            });
        }

        await _tokenService.RevokeRefreshTokenAsync(user, tokenToRevoke.TokenValue);

        if (tokenToRevoke.DeviceId == GetCurrentDeviceId())
        {
            _cookieManager.ClearAuthCookies();
        }

        return NoContent();
    }

    private async Task<(Token AccessToken, RefreshToken RefreshToken)> GenerateSetTokensAsync(User user,
        string deviceId, string deviceName, string ipAddress, bool rememberUser)
    {
        var accessToken = await _tokenService.GenerateAccessTokenAsync(user);
        var refreshToken = _tokenService.GenerateRefreshToken(deviceId, deviceName, ipAddress, rememberUser);

        _cookieManager.SetAuthCookies(accessToken, refreshToken);

        await _tokenService.StoreRefreshTokenAsync(user, refreshToken);

        return (accessToken, refreshToken);
    }

    private string GetClientIpAddress()
    {
        return HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
    }

    private string GetCurrentDeviceId()
    {
        return HttpContext.Request.Headers["X-Device-Id"].ToString();
    }

    private (string deviceId, string deviceName, string ipAddress) GetDeviceInfo()
    {
        var deviceId = Request.Headers["X-Device-Id"].ToString();
        var deviceName = Request.Headers["X-Device-Name"].ToString();
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

        return (deviceId, deviceName, ipAddress);
    }
}