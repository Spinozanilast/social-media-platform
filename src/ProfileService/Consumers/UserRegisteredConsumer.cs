using MassTransit;
using ProfileService.Common.Repositories;
using Shared.Models;

namespace ProfileService.Consumers;

public class UserRegisteredConsumer(ILogger<UserRegisteredConsumer> logger, IProfileRepository profileRepository)
    : IConsumer<UserRegistered>
{
    public async Task Consume(ConsumeContext<UserRegistered> context)
    {
        logger.LogInformation("Consuming message created for user with id: {id}", context.Message.UserId);
        var userRegisteredMessage = context.Message;
        await profileRepository.InitUserProfileAsync(userRegisteredMessage.UserId, userRegisteredMessage.References);
    }
}