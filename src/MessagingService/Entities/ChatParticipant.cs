using MessagingService.Data.Configurations;
using MessagingService.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace MessagingService.Entities;

[EntityTypeConfiguration(typeof(ChatParticipantsConfiguration))]
public class ChatParticipant
{
    public Guid ChatId { get; init; }
    public string UserId { get; init; }
    public Chat Chat { get; set; } = null!;
    public ActivityState ActivityState { get; set; }
}