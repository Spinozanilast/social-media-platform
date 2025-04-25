using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfileService.Common.Repositories;
using ProfileService.Contracts;
using ProfileService.Contracts.Mappers;

namespace ProfileService.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class ProfilesController(IProfileRepository profileRepository, ICountriesRepository countriesRepository)
    : ControllerBase
{
    private readonly IProfileRepository _profileRepository = profileRepository;
    private readonly ICountriesRepository _countriesRepository = countriesRepository;

    [Authorize]
    [HttpPost("{userId:guid}/init")]
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
    [HttpPut("{profileId:guid}/update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> SaveProfile([FromRoute] Guid profileId, [FromBody] SaveProfileDto dto)
    {
        await _profileRepository.SaveProfileAsync(dto.ToProfile(profileId));
        return Ok();
    }

    [HttpGet("{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProfile([FromRoute] Guid userId)
    {
        var profile = await _profileRepository.GetProfileAsync(userId);

        if (profile is null)
        {
            return NotFound();
        }

        return Ok(profile);
    }

    [Authorize]
    [HttpDelete("{userId:guid}/delete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteProfile([FromRoute] Guid userId)
    {
        await _profileRepository.DeleteProfileAsync(userId);
        return NoContent();
    }

    [HttpGet("countries")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCountries()
    {
        var countries = await _countriesRepository.GetAll();
        return Ok(countries);
    }
}