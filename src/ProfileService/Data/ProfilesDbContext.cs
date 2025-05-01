using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ProfileService.Data;

public class ProfilesDbContext(DbContextOptions<ProfilesDbContext> options)
    : DbContext(options)
{
    public DbSet<Entities.Profile> Profiles { get; set; }
    public DbSet<Entities.Country> Countries { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(warnings => warnings.Log(RelationalEventId.PendingModelChangesWarning));
    }
}