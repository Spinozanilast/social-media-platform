using AuthorizationService.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationService.Data;

public class IdentityAppContext : IdentityDbContext<User, Role, Guid>
{
    public IdentityAppContext()
    {
    }

    public IdentityAppContext(DbContextOptions<IdentityAppContext> options) : base(options)
    {
    }
}