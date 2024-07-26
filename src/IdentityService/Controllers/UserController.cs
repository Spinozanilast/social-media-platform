using IdentityService.Contracts.Authentication;
using IdentityService.Contracts.Login;
using IdentityService.Contracts.Registration;
using IdentityService.Entities;
using IdentityService.Mapping;
using IdentityService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
    [HttpPost(ApiEndpoints.AccountEndpoints.SignUp)]
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
    [HttpPost(ApiEndpoints.AccountEndpoints.Signin)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Authenticate([FromBody] LoginRequest loginRequest)
    {
        var user = await _userManager.FindByEmailAsync(loginRequest.Email);

        if (user is null || !await _userManager.CheckPasswordAsync(user, loginRequest.Password))
        {
            return Unauthorized();
        }
        
        return Ok(user.ToLoginResponse(_tokenService.GenerateToken(user)));
    }
}
