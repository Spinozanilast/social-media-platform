using Authentication.Configuration;
using IdentityService.Common.Services;
using IdentityService.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
public class AuthController(
    ITokenService tokenService,
    ICookiesService cookiesService,
    IUserService userService)
    : ControllerBase
{
    private readonly ITokenService _tokenService = tokenService;
    private readonly ICookiesService _cookiesService = cookiesService;
    private readonly IUserService _userService = userService;

    [Authorize]
    [HttpGet(IdentityApiEndpoints.AuthenticationEndpoints.ValidateTokenRelation)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ValidateTokenRelationToId([FromRoute] string idOrUsername)
    {
        if (_cookiesService.TryGetCookie(Request.Cookies, AuthCookieTypes.JwtCookie, out var cookie))
        {
            var userWithSlugType = await _userService.GetUserByIdOrUsernameWithSlugTypeAsync(idOrUsername);

            if (!userWithSlugType.IsSuccess) return BadRequest("User not found");

            var tokenValidator = new TokenValidator(JwtExtensions.GetTokenValidationParameters());

            if (tokenValidator.IsTokenValidByClaim(cookie,
                    userWithSlugType.Value.userSlugType.GetClaimNameFromSlugType(),
                    idOrUsername))
            {
                return Ok();
            }
        }

        return Unauthorized("You are not authorized to access this resource");
    }
}