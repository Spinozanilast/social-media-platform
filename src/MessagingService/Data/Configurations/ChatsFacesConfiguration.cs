using MessagingService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MessagingService.Data.Configurations;

public class ChatsFacesConfiguration : IEntityTypeConfiguration<ChatsFace>
{
    public void Configure(EntityTypeBuilder<ChatsFace> builder)
    {
        builder
            .HasKey(cf => new { cf.UserId, cf.Username });
        builder
            .HasIndex(cf => cf.UserId).HasDatabaseName("idx_chats_faces_user_id");
    }
}