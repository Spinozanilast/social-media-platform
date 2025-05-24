using AuthorizationService.Entities;
using AuthorizationService.Entities.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthorizationService.Data.Configurations;

public class UsersConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .ComplexProperty(u => u.GithubInfo,
                i => i.IsRequired());
        
        builder
            .OwnsMany<RefreshToken>(u => u.RefreshTokens)
            .HasIndex(r => r.DeviceName)
            .HasDatabaseName("idx_refreshTokens_deviceName");
    }
}