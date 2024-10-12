using IdentityService.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Data;

public class IdentityAppContext(DbContextOptions<IdentityAppContext> options)
    : IdentityDbContext<User, Role, Guid>(options);