using IdentityService.Entities;
using IdentityService.Entities.Enums;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Helpers.Extensions;

public static class UserSearcherExtensions
{
    public static async ValueTask<(User? User, UserSlugTypes SlugType)> FindUserBySlugWithTypeAsync(
        this UserManager<User> userManager,
        string slug)
    {
        if (Guid.TryParse(slug, out var id))
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            return (user, UserSlugTypes.Guid);
        }

        var userByName = await userManager.FindByNameAsync(slug);
        return (userByName, UserSlugTypes.UserName);
    }

    public static async ValueTask<User?> FindUserBySlugAsync(
        this UserManager<User> userManager,
        string slug)
    {
        var (user, _) = await userManager.FindUserBySlugWithTypeAsync(slug);
        return user;
    }
}