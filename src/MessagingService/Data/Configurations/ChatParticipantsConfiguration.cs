using MessagingService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MessagingService.Data.Configurations;

public class ChatParticipantsConfiguration : IEntityTypeConfiguration<ChatParticipant>
{
    public void Configure(EntityTypeBuilder<ChatParticipant> builder)
    {
        builder
            .HasKey(cp => new { cp.ChatId, cp.UserId });

        builder.HasOne(cp => cp.Chat).WithMany(ch => ch.Participants).HasForeignKey(cp => cp.ChatId);
    }
}