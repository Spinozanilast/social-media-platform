using AuthorizationService.Entities;
using Microsoft.AspNetCore.Identity;

namespace AuthorizationService.Data;

public static class DbContextSeeder
{
    public static async Task SeedRoles(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

        foreach (var role in IdentityRoles.GetRoles())
        {
            if (!await roleManager.RoleExistsAsync(role.Name!))
            {
                await roleManager.CreateAsync(role);
            }
        }
    }
}