namespace StoriesService.Models;

public record CreateStoryModel(Guid AuthorId, string Title, string Content, List<string> Tags, bool IsShared = false);