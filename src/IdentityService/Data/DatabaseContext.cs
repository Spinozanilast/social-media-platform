using IdentityService.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Data;

public class IdentityDbContext(DbContextOptions<IdentityDbContext> options)
    : IdentityDbContext<User, Role, Guid>(options);
