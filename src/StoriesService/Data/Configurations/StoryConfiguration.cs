using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoriesService.Entities;

namespace StoriesService.Data.Configurations;

public class StoryConfiguration : IEntityTypeConfiguration<Story>
{
    public void Configure(EntityTypeBuilder<Story> builder)
    {
        builder
            .Property(s => s.Title)
            .HasMaxLength(100);

        builder
            .Property(s => s.Content)
            .IsRequired();

        builder
            .Property(s => s.CreatedAt)
            .HasColumnType("timestamp")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder
            .Property(s => s.UpdatedAt)
            .HasColumnType("timestamp")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder
            .Property(s => s.OriginalPostId)
            .IsRequired(false);

        builder.HasOne(s => s.OriginalPost)
            .WithMany()
            .HasForeignKey(s => s.OriginalPostId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(s => s.AuthorId);
        builder.HasIndex(s => s.Tags);

        builder.Property(s => s.CreatedAt)
            .HasConversion(v => v.ToLocalTime(), v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
        builder.Property(s => s.UpdatedAt)
            .HasConversion(v => v.ToLocalTime(), v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
    }
}