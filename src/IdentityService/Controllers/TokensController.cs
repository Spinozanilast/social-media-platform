using IdentityService.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers;

[Authorize]
[ApiController]
public class TokensController : ControllerBase
{
    private readonly UserManager<User> _userManager;

    public TokensController(UserManager<User> userManager)
    {
        _userManager = userManager;
    }


    [Authorize]
    [HttpPost(IdentityApiEndpoints.TokensEndpoints.GetRefreshTokens)]
    public async Task<IActionResult> GetRefreshTokens([FromRoute]string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            return NotFound();
        }

        return Ok(user.RefreshTokens);
    }
}