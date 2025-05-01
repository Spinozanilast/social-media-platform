using MessagingService.Common.Repositories;
using MessagingService.Common.Services;
using MessagingService.Entities;

namespace MessagingService.Services;

public class ChatService : IChatService
{
    private readonly IChatsRepository _chatsRepository;
    private readonly IChatParticipantsRepository _chatParticipantsRepository;

    public ChatService(IChatsRepository chatsRepository,
        IChatParticipantsRepository chatParticipantsRepository)
    {
        _chatsRepository = chatsRepository;
        _chatParticipantsRepository = chatParticipantsRepository;
    }

    public async Task<Chat> GetOrCreateChatAsync(Guid currentUserId, Guid otherUserId)
    {
        var chat = await _chatsRepository.GetDuoChatAsync(currentUserId, otherUserId);
        if (chat is not null) return chat;

        chat = new Chat
        {
            Participants = new List<ChatParticipant>
            {
                new ChatParticipant { UserId = currentUserId },
                new ChatParticipant { UserId = otherUserId }
            }
        };

        await _chatsRepository.CreateChatAsync(chat);

        return chat;
    }

    public Task<List<Chat>> GetChatsAsync(Guid currentUserId)
    {
        return _chatsRepository.GetUserChatsAsync(currentUserId);
    }
}