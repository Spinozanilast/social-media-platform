using IdentityService.Entities;

namespace IdentityService.Data;

public static class DbContextSeeder
{
    public static async Task SeedRoles(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IdentityAppContext>();
        var rolesSet = context.Roles;

        foreach (var identityRole in IdentityRoles.GetRoles())
        {
            if (rolesSet.Any(role => role.Name == identityRole.Name)) continue;
            await rolesSet.AddAsync(identityRole);
        }

        await context.SaveChangesAsync();
    }
}