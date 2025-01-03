using IdentityService.Entities;
using IdentityService.Entities.Enums;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Helpers;

public static class UserSearcherExtensions
{
    public static async ValueTask<(User? user, UserSlugTypes slugType)> FindUserByUserNameThenIdWithSlugTypeAsync(
        this UserManager<User> userManager,
        string idOrUsername)
    {
        return Guid.TryParse(idOrUsername, out var id)
            ? (await userManager.FindByIdAsync(id.ToString()), UserSlugTypes.Guid)
            : (await userManager.FindByNameAsync(idOrUsername), UserSlugTypes.UserName);
    }

    public static async ValueTask<User?> FindUserByUserNameThenIdAsync(
        this UserManager<User> userManager,
        string idOrUsername)
    {
        return Guid.TryParse(idOrUsername, out _)
            ? await userManager.FindByIdAsync(idOrUsername)
            : await userManager.FindByNameAsync(idOrUsername);
    }
}