using IdentityService.Contracts;
using IdentityService.Contracts.Login;
using IdentityService.Contracts.Registration;
using IdentityService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly IUserService _userService;

    public AccountsController(IUserService userService)
    {
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpPost(IdentityApiEndpoints.AccountEndpoints.Register)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterUser([FromBody] UserForRegistration userForRegistration)
    {
        var responseResult = await _userService.RegisterUserAsync(userForRegistration);
        return responseResult.IsSuccess ? Created() : BadRequest(responseResult);
    }

    [AllowAnonymous]
    [HttpPost(IdentityApiEndpoints.AccountEndpoints.Login)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> LoginUser([FromBody] LoginRequest loginRequest)
    {
        var result = await _userService.LoginUserAsyncWithCookiesSet(loginRequest, response: Response);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    [Authorize]
    [HttpPost(IdentityApiEndpoints.AccountEndpoints.SignOut)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public new async Task<IActionResult> SignOut()
    {
        var response = await _userService.SignOut(Request, Response);
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpGet(IdentityApiEndpoints.AccountEndpoints.GetUserByIdOrUsername)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUser([FromRoute] string idOrUsername)
    {
        var result = await _userService.GetUserByIdOrUsernameAsync(idOrUsername);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }


    [AllowAnonymous]
    [HttpGet(IdentityApiEndpoints.AccountEndpoints.GetAll)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllUsers()
    {
        var result = await _userService.GetAllUserAsync();
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    [Authorize]
    [HttpPut(IdentityApiEndpoints.AccountEndpoints.UpdateUser)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateUserIdentity([FromRoute] string id, [FromBody] UserUpdateDto updatedUser)
    {
        var result = await _userService.UpdateUserIdentityAsync(id, updatedUser);
        return result.IsSuccess ? NoContent() : NotFound(result.Error);
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