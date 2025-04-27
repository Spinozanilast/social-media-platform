using Amazon.S3;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProfileService.Common.Services;

namespace ProfileService.Endpoints;

public static class ProfilesImagesEndpoints
{
    public static RouteGroupBuilder MapProfilesImagesEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost("/upload", async Task<Results<Created, BadRequest>> (
                [FromRoute] Guid userId,
                IFormFile profileImage,
                [FromServices] IProfileImageService imageService) =>
            {
                var isSuccess = await imageService.UploadProfileImageAsync(profileImage, userId);

                return isSuccess
                    ? TypedResults.Created()
                    : TypedResults.BadRequest();
            })
            .RequireAuthorization()
            .WithName("UploadProfileImage")
            .WithOpenApi();

        group.MapGet("", async Task<Results<FileStreamHttpResult, NotFound<string>>> (
                [FromRoute] Guid userId,
                [FromServices] IProfileImageService imageService) =>
            {
                try
                {
                    var result = await imageService.GetProfileImageAsync(userId);

                    return TypedResults.File(result.ResponseStream, result.Headers.ContentType);
                }
                catch (AmazonS3Exception ex)
                {
                    return TypedResults.NotFound("There was an error retrieving the profile image.");
                }
            })
            .AllowAnonymous()
            .WithName("GetProfileImage")
            .WithOpenApi();

        group.MapDelete("/delete", async Task<Results<NoContent, NotFound>> (
                [FromRoute] Guid userId,
                [FromServices] IProfileImageService imageService) =>
            {
                var result = await imageService.RemoveProfileImageAsync(userId);

                return result ? TypedResults.NoContent() : TypedResults.NotFound();
            })
            .RequireAuthorization()
            .WithName("RemoveProfileImage")
            .WithOpenApi();

        return group;
    }
}