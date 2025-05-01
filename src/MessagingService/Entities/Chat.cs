using MessagingService.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace MessagingService.Entities;

[EntityTypeConfiguration(typeof(ChatsConfiguration))]
public class Chat
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    public string? Name { get; set; }
    public string? BackgroundUrl { get; set; }

    public IList<Message> Messages { get; set; } = new List<Message>();
    public IList<ChatParticipant> Participants { get; set; } = new List<ChatParticipant>();
}