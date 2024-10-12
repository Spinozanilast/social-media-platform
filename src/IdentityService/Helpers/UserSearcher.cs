﻿using IdentityService.Entities;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Helpers;

public static class UserSearcher
{
    public static async Task<User?> FindUserByUserNameThenId(this UserManager<User> userManager, string idOrUsername)
    {
        var user = Guid.TryParse(idOrUsername, out _)
            ? await userManager.FindByNameAsync(idOrUsername)
            : await userManager.FindByIdAsync(idOrUsername);
        return user;
    }
}