using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProfileService.Common.Repositories;
using ProfileService.Contracts;
using ProfileService.Contracts.Mappers;
using ProfileService.Entities;

namespace ProfileService.Endpoints;

public static class ProfilesEndpoints
{
    public static RouteGroupBuilder MapProfilesEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost("/{userId:guid}/stories", async Task<Results<Ok, BadRequest<string>>> (
                [FromBody] Guid userId,
                [FromServices] IProfileRepository profilesRepository) =>
            {
                if (await profilesRepository.GetProfileAsync(userId) is not null)
                {
                    return TypedResults.BadRequest("Profile already exists");
                }

                await profilesRepository.InitUserProfileAsync(userId, null);

                return TypedResults.Ok();
            })
            .RequireAuthorization()
            .WithName("InitUserProfile");

        group.MapPut("{profileId:guid}/update", async Task<Ok> (
                [FromRoute] Guid profileId,
                [FromBody] SaveProfileDto dto,
                [FromServices] IProfileRepository profilesRepository) =>
            {
                await profilesRepository.UpdateProfileAsync(dto.ToProfile(profileId));
                return TypedResults.Ok();
            })
            .RequireAuthorization()
            .WithName("UpdateProfile");

        group.MapGet("{profileId:guid}", async Task<Results<Ok<Profile>, NotFound>> (
                [FromRoute] Guid profileId,
                [FromServices] IProfileRepository profilesRepository) =>
            {
                var profile = await profilesRepository.GetProfileAsync(profileId);

                return profile is not null ? TypedResults.Ok(profile) : TypedResults.NotFound();
            })
            .AllowAnonymous()
            .WithName("GetProfile");

        group.MapDelete("{profileId:guid}/delete", async Task<NoContent> (
                [FromRoute] Guid profileId,
                [FromServices] IProfileRepository profilesRepository) =>
            {
                await profilesRepository.DeleteProfileAsync(profileId);
                return TypedResults.NoContent();
            })
            .RequireAuthorization()
            .WithName("DeleteProfile");

        group.MapOpenApi();

        return group;
    }
}