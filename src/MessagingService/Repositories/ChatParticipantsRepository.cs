using MessagingService.Common.Repositories;
using MessagingService.Data;
using MessagingService.Entities;
using Microsoft.EntityFrameworkCore;

namespace MessagingService.Repositories;

public class ChatParticipantsRepository(MessagingDbContext dbContext) : IChatParticipantsRepository
{
    private readonly MessagingDbContext _dbContext = dbContext;

    public async Task InsertAsync(ChatParticipant participant)
    {
        var chat = await _dbContext
            .Chats
            .Include(chat => chat.Participants)
            .FirstOrDefaultAsync(c => c.Id == participant.ChatId);

        chat?.Participants.Add(participant);
    }
}