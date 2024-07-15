using IdentityService.Contracts.Api.Registration;
using IdentityService.Entities;
using IdentityService.Mapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers;

[ApiController]
public class AccountsController : ControllerBase
{
    private readonly UserManager<User> _userManager;

    public AccountsController(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    [HttpPost(ApiEndpoints.AccountEndpoints.SignUp)]
    public async Task<IActionResult> RegisterUser([FromBody] UserForRegistration userForRegistration)
    {
        if (userForRegistration is null)
        {
            return BadRequest();
        }

        var user = userForRegistration.ToUserWithoutHashPassword();
        user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, userForRegistration.Password);

        var result = await _userManager.CreateAsync(user);

        if (result.Succeeded) return Created();
        
        var errors = result.Errors.Select(e => e.Description);
        return BadRequest(new RegistrationResponse { Errors = errors });

    }
}
