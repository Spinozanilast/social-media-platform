using Microsoft.EntityFrameworkCore;
using ProfileService.Entities;

namespace ProfileService.Data;

public class ProfileDbContext(DbContextOptions<ProfileDbContext> options)
    : DbContext(options)
{
    public DbSet<Profile?> Profiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}