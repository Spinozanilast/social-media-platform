using FluentValidation.Results;
using StoriesService.Entities;

namespace StoriesService.Common;

public interface IStoryService
{
    Task<IEnumerable<Story>> GetAllStoriesAsync(string? tag = null, Guid? authorId = null, int pageNumber = 1,
        int pageSize = 10);
    
    Task<int> GetAllStoriesCountAsync(string? tag = null, Guid? authorId = null);

    Task<Story?> GetStoryByIdAsync(int id);
    Task<ValidationResult> AddStoryAsync(Story story);
    Task<ValidationResult> UpdateStoryAsync(Story story);
    Task DeleteStoryAsync(int id);
}