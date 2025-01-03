using IdentityService.Common.Services;
using IdentityService.Contracts;
using IdentityService.Contracts.Login;
using IdentityService.Contracts.Registration;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace IdentityService.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
public class AccountsController(IUserService userService, IPublishEndpoint publishEndpoint) : ControllerBase
{
    private readonly IUserService _userService = userService;
    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

    [AllowAnonymous]
    [HttpPost(IdentityApiEndpoints.AccountEndpoints.Register)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterUser([FromBody] UserForRegistration userForRegistration)
    {
        var responseResult = await _userService.RegisterUserAsync(userForRegistration);

        if (!responseResult.IsSuccess) return BadRequest(responseResult.ErrorValue);

        var registeredUser = new UserRegistered(responseResult.Value.UserId);
        await _publishEndpoint.Publish(registeredUser);
        return Created();
    }

    [AllowAnonymous]
    [HttpPost(IdentityApiEndpoints.AccountEndpoints.Login)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> LoginUser([FromBody] LoginRequest loginRequest)
    {
        var result = await _userService.LoginUserAsyncWithCookiesSetAsync(loginRequest, response: Response);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    [Authorize]
    [HttpPost(IdentityApiEndpoints.AccountEndpoints.SignOut)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public new async Task<IActionResult> SignOut()
    {
        await _userService.SignOut(Request, Response);
        return Ok();
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