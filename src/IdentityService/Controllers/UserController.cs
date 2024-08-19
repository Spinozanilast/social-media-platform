using IdentityService.Contracts;
using IdentityService.Contracts.Authentication;
using IdentityService.Contracts.Registration;
using IdentityService.Entities;
using IdentityService.Mapping;
using IdentityService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Controllers;

[Authorize]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly TokenService _tokenService;

    public AccountsController(UserManager<User> userManager, TokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
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

        return BadRequest(result.ToRegistrationResponse());
    }

    [AllowAnonymous]
    [HttpPost(IdentityApiEndpoints.AccountEndpoints.Login)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> LoginUser([FromBody] LoginRequest loginRequest)
    {
        var user = await _userManager.FindByEmailAsync(loginRequest.Email);

        if (user is null || !await _userManager.CheckPasswordAsync(user, loginRequest.Password))
        {
            return NotFound();
        }
        
        Response.Cookies.Append("jwt", _tokenService.GenerateToken(user), new CookieOptions
        {
            HttpOnly = true
        });
        return Ok(user.ToLoginResponse());
    }

    [AllowAnonymous]
    [HttpGet(IdentityApiEndpoints.AccountEndpoints.GetUserByIdOrUsername)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUser([FromRoute] string idOrUsername)
    {
        var user = await GetUserByIdOrUsername(idOrUsername);

        if (user is null)
        {
            return NotFound();
        }

        return Ok(user.ToUserGetModel());
    }


    [AllowAnonymous]
    [HttpGet(IdentityApiEndpoints.AccountEndpoints.GetAll)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

    private async Task<User?> GetUserByIdOrUsername(string idOrUsername)
    {
        var user = Guid.TryParse(idOrUsername, out var id)
            ? await _userManager.FindByIdAsync(idOrUsername)
            : await _userManager.FindByNameAsync(idOrUsername);
        return user;
    }
}