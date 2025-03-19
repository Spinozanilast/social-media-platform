namespace IdentityService.Entities;

public static class IdentityRoles
{
    public static Role[] GetRoles() => [AdminRole, ModeratorRole, UserRole];

    public static Role AdminRole => new("Admin");
    public static Role ModeratorRole => new("Moderator");
    public static Role UserRole => new("User");
}