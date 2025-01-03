using Microsoft.EntityFrameworkCore;
using StoriesService.Data.Configurations;
using StoriesService.Entities;

namespace StoriesService.Data;

public class StoriesDbContext(DbContextOptions<StoriesDbContext> dbContextOptions) : DbContext(dbContextOptions)
{
    public DbSet<Story> Stories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new StoryConfiguration());
    }
}