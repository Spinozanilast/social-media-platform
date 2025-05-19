using FluentValidation.Results;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using StoriesService.Common;
using StoriesService.Common.Mappers;
using StoriesService.Entities;
using StoriesService.Models;

namespace StoriesService;

public static class Endpoints
{
    public static RouteGroupBuilder MapStoriesEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("", async Task<Ok<IEnumerable<Story>>> (
                [AsParameters] StoryQueryParams queryParams,
                [FromServices] IStoryService storyService) =>
            {
                var result = await storyService.GetAllStoriesAsync(
                    queryParams.Tag,
                    queryParams.AuthorId,
                    queryParams.PageNumber,
                    queryParams.PageSize
                );
                return TypedResults.Ok(result);
            })
            .AllowAnonymous()
            .WithName("GetAllStories");

        group.MapGet("/count", async (
                [FromQuery] string? tag,
                [FromQuery] Guid? authorId,
                [FromServices] IStoryService storyService) =>
            {
                var count = await storyService.GetAllStoriesCountAsync(tag, authorId);
                return TypedResults.Ok(count);
            })
            .AllowAnonymous()
            .WithName("GetStoriesCount");

        group.MapGet("/{id:int}", async Task<Results<Ok<Story>, NotFound>> (
                [FromRoute] int id,
                [FromServices] IStoryService storyService) =>
            {
                var story = await storyService.GetStoryByIdAsync(id);
                return story is null
                    ? TypedResults.NotFound()
                    : TypedResults.Ok(story);
            })
            .AllowAnonymous()
            .WithName("GetStoryById");

        group.MapPost("/", async Task<Results<CreatedAtRoute<Story>, BadRequest<List<ValidationFailure>>>> (
                [FromBody] CreateStoryModel model,
                [FromServices] IStoryService storyService) =>
            {
                var story = model.ToStory();
                var result = await storyService.AddStoryAsync(story);

                return result.IsValid
                    ? TypedResults.CreatedAtRoute(story, "GetStoryById", new { id = story.Id })
                    : TypedResults.BadRequest(result.Errors);
            })
            .RequireAuthorization()
            .WithName("CreateStory");

        group.MapPut("/{id:int}", async Task<Results<NoContent, BadRequest<List<ValidationFailure>>, NotFound>> (
                [FromRoute] int id,
                [FromBody] UpdateStoryModel model,
                [FromServices] IStoryService storyService) =>
            {
                var story = await storyService.GetStoryByIdAsync(id);
                if (story is null) return TypedResults.NotFound();

                model.UpdateStory(story);
                var result = await storyService.UpdateStoryAsync(story);

                return result.IsValid
                    ? TypedResults.NoContent()
                    : TypedResults.BadRequest(result.Errors);
            })
            .RequireAuthorization()
            .WithName("UpdateStory");

        group.MapDelete("/{id:int}", async (
                [FromRoute] int id,
                [FromServices] IStoryService storyService) =>
            {
                await storyService.DeleteStoryAsync(id);
                return TypedResults.NoContent();
            })
            .RequireAuthorization()
            .WithName("DeleteStory");

        group.MapOpenApi();
        
        return group;
    }
}

public record StoryQueryParams(
    string? Tag = null,
    Guid? AuthorId = null,
    int PageNumber = 1,
    int PageSize = 10);