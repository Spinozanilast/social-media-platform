using Authentication.Configuration;
using IdentityService.Contracts;
using IdentityService.Contracts.Login;
using IdentityService.Contracts.Registration;
using IdentityService.Entities;
using IdentityService.Mapping;
using IdentityService.Services;
using IdentityService.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly UserUtilities _userUtilities;
    private readonly ITokenService _tokenService;
    private readonly SignInManager<User> _signInManager;

    public AccountsController(UserManager<User> userManager, ITokenService tokenService, UserUtilities userUtilities,
        SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _userUtilities = userUtilities;
        _signInManager = signInManager;
    }

    [AllowAnonymous]
    [HttpPost(IdentityApiEndpoints.AccountEndpoints.Register)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterUser([FromBody] UserForRegistration userForRegistration)
    {
        var user = userForRegistration.ToUserWithoutHashPassword();
        user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, userForRegistration.Password);

        var result = await _userManager.CreateAsync(user);

        if (result.Succeeded) return Created();

        return BadRequest(result.ToDefaultErrorResponse());
    }

    [AllowAnonymous]
    [HttpPost(IdentityApiEndpoints.AccountEndpoints.Login)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> LoginUser([FromBody] LoginRequest loginRequest)
    {
        var user = await _userManager.FindByEmailAsync(loginRequest.Email);

        if (user is null)
        {
            return NotFound("User with such email was not found");
        }

        var signInResult = await _signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, false);

        if (!signInResult.Succeeded)
        {
            return NotFound("Password or email is wrong");
        }

        var jwtToken = await _tokenService.GenerateJwtToken(user);
        SetHttpOnlyCookie(TokensConstants.JwtCookieKey, jwtToken.TokenValue, jwtToken.ExpiryDate);
        var refreshToken = await _tokenService.GenerateRefreshTokenWithSave(user);
        SetHttpOnlyCookie(TokensConstants.RefreshCookieKey, refreshToken.TokenValue, refreshToken.ExpiryDate);

        var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
        return Ok(user.ToLoginResponse(rolesList));
    }

    [AllowAnonymous]
    [HttpGet(IdentityApiEndpoints.AccountEndpoints.GetUserByIdOrUsername)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUser([FromRoute] string idOrUsername)
    {
        var user = await _userUtilities.GetUserByIdOrUsername(idOrUsername);

        if (user is null)
        {
            return NotFound();
        }

        return Ok(user.ToUserGetModel());
    }


    [AllowAnonymous]
    [HttpGet(IdentityApiEndpoints.AccountEndpoints.GetAll)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllUsers()
    {
        if (!await _userManager.Users.AnyAsync())
        {
            return NotFound();
        }

        return Ok(_userManager.Users.Select(user => user.ToUserGetModel()));
    }

    [Authorize]
    [HttpPut(IdentityApiEndpoints.AccountEndpoints.UpdateUser)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateUserIdentity([FromRoute] string id, [FromBody] UserUpdateDto updatedUser)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user is null)
        {
            return NotFound("There is no such ID or Username.");
        }

        //TODO: check for existing users and complete endpoint
        return Ok();
    }

    private void SetHttpOnlyCookie(string key, string value, DateTime? expires)
    {
        var httpOnlyOption = new CookieOptions
        {
            HttpOnly = true,
        };

        if (expires.HasValue)
        {
            httpOnlyOption.Expires = expires.Value;
        }

        Response.Cookies.Append(key, value, httpOnlyOption);
    }

    private string? GetOrGenerateIoAddress()
    {
        const string key = "X-Forwarded-For";
        if (Request.Headers.ContainsKey(key))
        {
            return Request.Headers[key];
        }
        else
        {
            return HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
        }
    }
}