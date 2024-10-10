using IdentityService.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityService.Data;

public class IdentityAppContext(DbContextOptions<IdentityAppContext> options)
    : IdentityDbContext<User, Role, Guid>(options);