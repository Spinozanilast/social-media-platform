using FluentValidation;
using StoriesService.Common;
using StoriesService.Entities;
using StoriesService.Repositories;
using StoriesService.Services;
using StoriesService.Validators;

namespace StoriesService;

public static class ServicesInjector
{
    public static IServiceCollection AddStoriesServices(this IServiceCollection services, IConfiguration _)
    {
        services.AddScoped<IStoryRepository, StoryRepository>();
        services.AddTransient<IValidator<Story>, StoryValidator>();
        services.AddScoped<IStoryService, StoryService>();
        
        return services;
    }
}