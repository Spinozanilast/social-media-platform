using MessagingService.Entities;

namespace MessagingService.Common.Repositories;

public interface IMessagesRepository
{
    Task AddMessageAsync(Message message);
}