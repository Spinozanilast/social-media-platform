using MessagingService.Entities;
using Microsoft.EntityFrameworkCore;

namespace MessagingService.Data;

public class MessagingDbContext : DbContext
{
    public DbSet<Chat> Chats { get; set; }
    public DbSet<ChatParticipant> ChatParticipants { get; set; }
    public DbSet<ChatsFace> ChatsFaces { get; set; }
    public DbSet<Message> Messages { get; set; }

    public MessagingDbContext()
    {
    }

    public MessagingDbContext(DbContextOptions<MessagingDbContext> options) : base(options)
    {
    }
}