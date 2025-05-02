using MessagingService.Entities;

namespace MessagingService.Common.Repositories;

public interface IChatsRepository
{
    Task<List<Chat>> GetUserChatsAsync(string currentUserId);
    Task<List<Chat>> GetRelatedChatsAsync(string currentUserId, string otherUserId);
    Task<Chat?> GetDuoChatAsync(string currentUserId, string otherUserId);
    Task CreateChatAsync(Chat chat);
}