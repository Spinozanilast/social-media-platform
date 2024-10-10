using Authentication.Configuration;
using IdentityService.Entities;
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRefreshTokens([FromRoute] string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            return NotFound();
        }

        return Ok(user.RefreshTokens);
    }

    [AllowAnonymous]
    [HttpPost(IdentityApiEndpoints.TokensEndpoints.RefreshToken)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshToken([FromRoute] string idOrUsername)
    {
        var user = await _userUtilities.GetUserByIdOrUsername(idOrUsername);

        if (user is null)
        {
            return NotFound("Such user was not found");
        }

        if (!Request.Cookies.TryGetValue(TokensConstants.RefreshCookieKey, out var currentRefreshToken) ||
            string.IsNullOrEmpty(currentRefreshToken)) return Unauthorized();

        var tokenPair = await _tokenService.TryRefreshToken(user, currentRefreshToken);

        if (tokenPair is null)
        {
            return Unauthorized("Refresh token was expired");
        }

        var httpOnlyOption = new CookieOptions
        {
            HttpOnly = true,
        };

        Response.Cookies.Append(TokensConstants.JwtCookieKey, tokenPair.JwtToken.TokenValue, httpOnlyOption);
        Response.Cookies.Append(TokensConstants.RefreshCookieKey, tokenPair.RefreshToken.TokenValue, httpOnlyOption);

        return Ok();
    }
}