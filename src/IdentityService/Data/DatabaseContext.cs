using IdentityService.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Data;

public class UsersDbContext : IdentityDbContext<User                    >
{
    public UsersDbContext(DbContextOptions options) : base(options)
    {
    }
}
