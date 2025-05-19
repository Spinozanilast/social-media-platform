using MessagingService.Common.Repositories;
using MessagingService.Data;
using MessagingService.Entities;

namespace MessagingService.Repositories;

public class MessagesRepository(MessagingDbContext dbContext) : IMessagesRepository
{
    private readonly MessagingDbContext _dbContext = dbContext;

    public async Task AddMessageAsync(Message message)
    {
        await _dbContext.Messages.AddAsync(message);
        await _dbContext.SaveChangesAsync();
    }
}