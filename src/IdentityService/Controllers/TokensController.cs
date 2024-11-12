using Authentication.Configuration;
using IdentityService.Entities;
using IdentityService.Helpers;
using IdentityService.Services;
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
    private readonly ICookiesService _cookiesService;

    public TokensController(UserManager<User> userManager, ITokenService tokenService, ICookiesService cookiesService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _cookiesService = cookiesService;
    }

    [HttpPost(IdentityApiEndpoints.TokensEndpoints.CheckToken)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CheckJwtTokenAvailability()
    {
        if (_cookiesService.JwtTokenExistsInRequest(Request.Cookies, AuthCookieTypes.JwtCookie))
        {
            return Ok();
        }

        return Unauthorized();
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

        if (Request.Cookies.TryGetValue(TokensConstants.GetCookieKey(AuthCookieTypes.RefreshCookie),
                out var refreshToken) &&
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

        if (!_cookiesService.TryGetCookie(Request.Cookies, AuthCookieTypes.RefreshCookie,
                out var currentRefreshToken) ||
            string.IsNullOrEmpty(currentRefreshToken)) return Unauthorized();

        var tokenPairResult = await _tokenService.TryRefreshToken(user, currentRefreshToken);

        if (!tokenPairResult.IsSuccess)
        {
            return Unauthorized(tokenPairResult.Error);
        }

        var (jwtToken, refreshToken) = tokenPairResult.Value;
        _tokenService.SetTokensInCookies(Response, jwtToken, refreshToken);

        return Ok();
    }

    [AllowAnonymous]
    [HttpPost(IdentityApiEndpoints.TokensEndpoints.RevokeToken)]
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

        var tokenAvailable =
            _cookiesService.TryGetCookie(Request.Cookies, AuthCookieTypes.RefreshCookie, out var refreshToken);

        if (tokenAvailable || string.IsNullOrEmpty(refreshToken))
            return BadRequest(new { message = "Token is required" });

        var response = _tokenService.TryRevokeToken(user, refreshToken);

        if (!response)
            return NotFound("Token not found");

        return Ok("Token revoked");
    }
}