using MessagingService.Entities;

namespace MessagingService.Common.Repositories;

public interface IChatParticipantsRepository
{
    Task InsertAsync(ChatParticipant participant);
}