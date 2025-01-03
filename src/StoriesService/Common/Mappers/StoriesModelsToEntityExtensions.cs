using StoriesService.Entities;
using StoriesService.Models;

namespace StoriesService.Common.Mappers;

public static class StoriesModelsToEntityExtensions
{
    public static Story ToStory(this CreateStoryModel model)
    {
        return new Story
        {
            AuthorId = model.AuthorId, Title = model.Title, Content = model.Content,
            Tags = model.Tags, IsShared = model.IsShared, CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public static Story UpdateStory(this UpdateStoryModel model, Story story)
    {
        story.Title = model.Title;
        story.Content = model.Content;
        story.Tags = model.Tags;
        story.IsEdited = true;
        story.IsShared = model.IsShared;
        story.UpdatedAt = model.UpdatedAt ?? DateTime.UtcNow;
        story.OriginalPostId = model.OriginalPostId;

        return story;
    }
}