using MessagingService.Entities;

namespace MessagingService.Common.Repositories;

public interface IChatsRepository
{
    Task<List<Chat>> GetUserChatsAsync(Guid currentUserId);
    Task<List<Chat>> GetRelatedChatsAsync(Guid currentUserId, Guid otherUserId);
    Task<Chat?> GetDuoChatAsync(Guid currentUserId, Guid otherUserId);
    Task CreateChatAsync(Chat chat);
}