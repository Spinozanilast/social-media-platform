using MessagingService.Entities;

namespace MessagingService.Common.Services;

public interface IChatService
{
    Task<Chat> GetOrCreateChatAsync(string currentUserId, string otherUserId);
    Task<List<Chat>> GetChatsAsync(string currentUserId);
}