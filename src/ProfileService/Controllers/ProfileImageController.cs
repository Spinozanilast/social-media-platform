using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfileService.Common.Services;
using ProfileService.Models;

namespace ProfileService.Controllers;

[Authorize]
[ApiController]
public class ProfileImageController(IProfileImageService imageService) : ControllerBase
{
    private readonly IProfileImageService _imageService = imageService;

    [Authorize]
    [HttpPost(ProfileApiEndpoints.ProfileImagesEndpoints.Upload)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadProfileImage([FromForm] Image profileImage, Guid userId)
    {
        var result = await _imageService.UploadProfileImageAsync(profileImage, userId);

        return result switch
        {
            true => CreatedAtAction(nameof(GetProfileImage), new { userId }),
            _ => BadRequest()
        };
    }

    [AllowAnonymous]
    [HttpGet(ProfileApiEndpoints.ProfileImagesEndpoints.Get)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProfileImage([FromRoute] Guid userId)
    {
        var result = await _imageService.GetProfileImageAsync(userId);

        if (result is null)
        {
            return BadRequest();
        }

        return Ok(File(result.ImageData, result.ContentType));
    }

    [Authorize]
    [HttpPost(ProfileApiEndpoints.ProfileImagesEndpoints.Remove)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveProfileImage([FromRoute] Guid userId)
    {
        var result = await _imageService.RemoveProfileImageAsync(userId);

        return result switch
        {
            true => NoContent(),
            _ => NotFound()
        };
    }
}