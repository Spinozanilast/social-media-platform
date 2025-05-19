using MessagingService.Common.Repositories;
using MessagingService.Common.Services;
using MessagingService.Repositories;
using MessagingService.Services;

namespace MessagingService;

public static class ServicesInjector
{
    public static IServiceCollection AddMessagingServices(this IServiceCollection services, IConfiguration _)
    {
        services.AddScoped<IChatsRepository, ChatsRepository>();
        services.AddScoped<IChatParticipantsRepository, ChatParticipantsRepository>();
        services.AddScoped<IMessagesRepository, MessagesRepository>();
        services.AddScoped<IChatService, ChatsService>();
        
        services.AddSignalR();

        return services;
    }
}