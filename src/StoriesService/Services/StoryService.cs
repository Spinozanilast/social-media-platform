using FluentValidation;
using FluentValidation.Results;
using StoriesService.Common;
using StoriesService.Entities;

namespace StoriesService.Services;

public class StoryService : IStoryService
{
    private readonly IStoryRepository _storyRepository;
    private readonly IValidator<Story> _storyValidator;

    public StoryService(IStoryRepository storyRepository, IValidator<Story> storyValidator)
    {
        _storyRepository = storyRepository;
        _storyValidator = storyValidator;
    }

    public async Task<IEnumerable<Story>> GetAllStoriesAsync(string? tag = null, Guid? authorId = null,
        int pageNumber = 1, int pageSize = 10)
    {
        return await _storyRepository.GetAllStoriesAsync(tag, authorId, pageNumber, pageSize);
    }

    public async Task<int> GetAllStoriesCountAsync(string? tag = null, Guid? authorId = null)
    {
        return await _storyRepository.GetAllStoriesCountAsync(tag, authorId);
    }

    public async Task<Story?> GetStoryByIdAsync(int id)
    {
        return await _storyRepository.GetStoryByIdAsync(id);
    }

    public async Task<ValidationResult> AddStoryAsync(Story story)
    {
        var validationResult = await _storyValidator.ValidateAsync(story);
        if (validationResult.IsValid)
        {
            await _storyRepository.AddStoryAsync(story);
        }

        return validationResult;
    }

    public async Task<ValidationResult> UpdateStoryAsync(Story story)
    {
        var validationResult = await _storyValidator.ValidateAsync(story);

        if (validationResult.IsValid)
        {
            await _storyRepository.UpdateStoryAsync(story);
        }

        return validationResult;
    }

    public async Task DeleteStoryAsync(int id)
    {
        await _storyRepository.DeleteStoryAsync(id);
    }
}