namespace IdentityService.Entities;

public enum Roles
{
    Administrator,
    Moderator,
    User
}

public static class IdentityRoles
{
    public static Role[] GetRoles() => [AdminRole, ModeratorRole, UserRole];
    
    public static Role AdminRole => new Role(Roles.Administrator.ToString());
    public static Role ModeratorRole => new(Roles.Moderator.ToString());
    public static Role UserRole => new(Roles.User.ToString());
}