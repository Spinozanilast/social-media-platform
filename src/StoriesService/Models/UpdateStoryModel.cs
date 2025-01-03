namespace StoriesService.Models;

public record UpdateStoryModel(
    string Title,
    string Content,
    List<string> Tags,
    bool IsShared = false,
    DateTime? UpdatedAt = null,
    int? OriginalPostId = null);