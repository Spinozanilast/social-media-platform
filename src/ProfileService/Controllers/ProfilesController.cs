using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfileService.Common.Extensions;
using ProfileService.Common.Repositories;
using ProfileService.Entities;
using ProfileService.Models;

namespace ProfileService.Controllers;

[ApiController]
public class ProfilesController(IProfileRepository profileRepository) : ControllerBase
{
    private readonly IProfileRepository _profileRepository = profileRepository;

    [Authorize]
    [HttpPost(ProfileApiEndpoints.ProfileEndpoints.Init)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> InitUserProfile([FromBody] Guid userId)
    {
        if (await _profileRepository.GetProfileAsync(userId) is not null)
        {
            return BadRequest("Profile already exists");
        }

        await _profileRepository.InitUserProfileAsync(userId);
        return Ok();
    }

    [Authorize]
    [HttpPut(ProfileApiEndpoints.ProfileEndpoints.Update)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> SaveProfile([FromBody] Profile profile)
    {
        await _profileRepository.SaveProfileAsync(profile);
        return Ok();
    }

    [HttpGet(ProfileApiEndpoints.ProfileEndpoints.Get)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProfile([FromRoute] Guid userId)
    {
        var profile = await _profileRepository.GetProfileAsync(userId);

        if (profile is null)
        {
            return NotFound();
        }

        return Ok(profile.GetProfileWithStringifiedInterests());
    }

    [Authorize]
    [HttpDelete(ProfileApiEndpoints.ProfileEndpoints.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteProfile([FromRoute] Guid userId)
    {
        await _profileRepository.DeleteProfileAsync(userId);
        return NoContent();
    }
}