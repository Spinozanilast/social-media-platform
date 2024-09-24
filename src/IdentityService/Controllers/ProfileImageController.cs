using IdentityService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers;

[Authorize]
[ApiController]
public class ProfileImageController : ControllerBase
{
    private readonly IProfileImageService _imageService;

    public ProfileImageController(IProfileImageService imageService)
    {
        _imageService = imageService;
    }

    [Authorize]
    [HttpPost(IdentityApiEndpoints.ProfileImagesEndpoints.Upload)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadProfileImage([FromForm] Photo profileImage, Guid userId)
    {
        if (profileImage is null)
        {
            return BadRequest();
        }

        var result = await _imageService.UploadProfileImageAsync(profileImage, userId);

        return result switch
        {
            true => CreatedAtAction(nameof(GetProfileImage), new { userId }),
            _ => BadRequest()
        };
    }

    [AllowAnonymous]
    [HttpGet(IdentityApiEndpoints.ProfileImagesEndpoints.Get)]
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
    [HttpPost(IdentityApiEndpoints.ProfileImagesEndpoints.Remove)]
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

public class Photo
{
    public IFormFile formFile { get; set; }
}