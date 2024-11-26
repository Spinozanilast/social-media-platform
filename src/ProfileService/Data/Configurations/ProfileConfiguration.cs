using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProfileService.Entities;

namespace ProfileService.Data.Configurations;

public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder
            .HasKey(p => p.UserId)
            .HasName("Id");

        builder
            .Property(p => p.References)
            .HasColumnType("varchar(80)[]");
    }
}