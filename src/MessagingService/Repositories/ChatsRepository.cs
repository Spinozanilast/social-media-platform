using MessagingService.Common.Repositories;
using MessagingService.Data;
using MessagingService.Entities;
using Microsoft.EntityFrameworkCore;

namespace MessagingService.Repositories;

public class ChatsRepository(MessagingDbContext dbContext) : IChatsRepository
{
    private readonly MessagingDbContext _dbContext = dbContext;

    public async Task<List<Chat>> GetUserChatsAsync(Guid currentUserId)
    {
        return await _dbContext
            .Chats
            .AsNoTracking()
            .Include(c => c.Participants)
            .Where(c => c.Participants.Any(p => p.UserId == currentUserId))
            .ToListAsync();
    }

    public async Task<List<Chat>> GetRelatedChatsAsync(Guid currentUserId, Guid otherUserId)
    {
        return await _dbContext
            .Chats
            .AsNoTracking()
            .Where(c =>
                c.Participants.Any(p => p.UserId == currentUserId && c.Participants.Any(p => p.UserId == otherUserId)))
            .ToListAsync();
    }

    public async Task<Chat?> GetDuoChatAsync(Guid currentUserId, Guid otherUserId)
    {
        return await _dbContext
            .Chats
            .AsNoTracking()
            .Where(c => c.Participants.Count == 2 && c.Participants.Any(p => p.UserId == currentUserId) &&
                        c.Participants.Any(p => p.UserId == otherUserId))
            .FirstOrDefaultAsync();
    }

    public async Task CreateChatAsync(Chat chat)
    {
        await _dbContext.Chats.AddAsync(chat);
        await _dbContext.SaveChangesAsync();
    }
}