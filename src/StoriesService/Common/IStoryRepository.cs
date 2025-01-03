using StoriesService.Entities;

namespace StoriesService.Common;

public interface IStoryRepository
{
    Task<IEnumerable<Story>> GetAllStoriesAsync(string? tag, Guid? authorId,
        int pageNumber, int pageSize);
    
    Task<int> GetAllStoriesCountAsync(string? tag, Guid? authorId);
    Task<Story?> GetStoryByIdAsync(int id);
    Task AddStoryAsync(Story story);
    Task UpdateStoryAsync(Story story);
    Task DeleteStoryAsync(int id);
}