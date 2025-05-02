using MessagingService.Common.Repositories;
using MessagingService.Common.Services;
using MessagingService.Data;
using MessagingService.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace MessagingService.Services;

[Authorize]
public class ChatHub : Hub
{
    private readonly MessagingDbContext _dbContext;
    private readonly IChatService _chatService;
    private readonly IMessagesRepository _messagesRepository;

    public ChatHub(MessagingDbContext dbContext, IChatService chatService, IMessagesRepository messagesRepository)
    {
        _dbContext = dbContext;
        _chatService = chatService;
        _messagesRepository = messagesRepository;
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        await base.OnConnectedAsync();
    }

    public async Task SendMessageAsync(string receiverId, string content)
    {
        var senderId = Context.UserIdentifier;
        var chat = await _chatService.GetOrCreateChatAsync(senderId, receiverId);

        var message = new Message
        {
            Content = content,
            SenderId = senderId,
            ChatId = chat.Id,
            SentAt = DateTime.UtcNow
        };

        await _messagesRepository.AddMessageAsync(message);
        await Clients.Users(senderId, receiverId).SendAsync("ReceiveMessage", message);
    }
}