using MessagingService.Entities;

namespace MessagingService.Common.Services;

public interface IChatService
{
    Task<Chat> GetOrCreateChatAsync(Guid currentUserId, Guid otherUserId);
    Task<List<Chat>> GetChatsAsync(Guid currentUserId);
}