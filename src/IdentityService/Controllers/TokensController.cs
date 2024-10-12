using Authentication.Configuration;
using IdentityService.Entities;
using IdentityService.Entities.Tokens;
using IdentityService.Helpers;
using IdentityService.Services;
using IdentityService.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers;

[Authorize]
[ApiController]
public class TokensController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenService _tokenService;

    public TokensController(UserManager<User> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    [Authorize]
    [HttpPost(IdentityApiEndpoints.TokensEndpoints.GetRefreshTokens)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRefreshTokens([FromRoute] string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            return NotFound();
        }

        if (Request.Cookies.TryGetValue(TokensConstants.RefreshCookieKey, out var refreshToken) &&
            _tokenService.GetUsersRefreshTokenActivityStatus(user, refreshToken))
        {
            return Ok(user.RefreshTokens);
        }

        return BadRequest("Those user refresh tokens are not yours. Go AWAY!");
    }

    [AllowAnonymous]
    [HttpPost(IdentityApiEndpoints.TokensEndpoints.RefreshToken)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshToken([FromRoute] string idOrUsername)
    {
        var user = await _userManager.FindUserByUserNameThenId(idOrUsername);

        if (user is null)
        {
            return NotFound("Such user was not found");
        }

        if (!Request.Cookies.TryGetValue(TokensConstants.RefreshCookieKey, out var currentRefreshToken) ||
            string.IsNullOrEmpty(currentRefreshToken)) return Unauthorized();

        var tokenPairResult = await _tokenService.TryRefreshToken(user, currentRefreshToken);

        if (!tokenPairResult.IsSuccess)
        {
            return Unauthorized(tokenPairResult.Error);
        }

        var (jwtToken, refreshToken) = tokenPairResult.Value;

        Response.Cookies.AppendHttpOnlyCookie(TokensConstants.JwtCookieKey, jwtToken.TokenValue,
            jwtToken.ExpiryDate);
        Response.Cookies.AppendHttpOnlyCookie(TokensConstants.RefreshCookieKey, refreshToken.TokenValue,
            refreshToken.ExpiryDate);

        return Ok();
    }

    [AllowAnonymous]
    [HttpPost(IdentityApiEndpoints.TokensEndpoints.RefreshToken)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RevokeToken([FromRoute] string idOrUsername)
    {
        var user = await _userManager.FindUserByUserNameThenId(idOrUsername);

        if (user is null)
        {
            return NotFound("Such user was not found");
        }

        var tokenAvailable = Request.Cookies.TryGetValue(TokensConstants.RefreshCookieKey, out var refreshToken);

        if (tokenAvailable || string.IsNullOrEmpty(refreshToken))
            return BadRequest(new { message = "Token is required" });
        
        var response = _tokenService.TryRevokeToken(user, refreshToken);

        if (!response)
            return NotFound("Token not found");

        return Ok("Token revoked");
    }
}