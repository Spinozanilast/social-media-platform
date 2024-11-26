using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProfileService.Data.Helpers;
using ProfileService.Entities;

namespace ProfileService.Data.Configurations;

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder
            .Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder
            .Property(c => c.IsoCode)
            .HasMaxLength(2)
            .IsRequired();

        builder.HasIndex(c => c.Name).IsUnique();

        builder.SeedCountriesTableWithData();
    }
}