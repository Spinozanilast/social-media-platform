using MessagingService.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using NpgsqlTypes;

namespace MessagingService.Entities;

[EntityTypeConfiguration(typeof(MessagesConfiguration))]
public class Message
{
    public int Id { get; init; }
    public string Content { get; set; } = null!;
    public NpgsqlTsVector SearchVector { get; set; } = null!;

    public DateTime SentAt { get; init; } = DateTime.UtcNow;

    public ChatParticipant Sender { get; set; } = null!;

    public Chat Chat { get; set; } = null!;
}