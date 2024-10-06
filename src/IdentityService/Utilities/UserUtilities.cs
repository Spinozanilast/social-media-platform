using IdentityService.Entities;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Utilities;

public class UserUtilities
{
    private readonly UserManager<User> _userManager;

    public UserUtilities(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
    }

    public async Task<User?> GetUserByIdOrUsername(string idOrUsername)
    {
        var user = Guid.TryParse(idOrUsername, out var id)
            ? await _userManager.FindByIdAsync(idOrUsername)
            : await _userManager.FindByNameAsync(idOrUsername);
        return user;
    }
}