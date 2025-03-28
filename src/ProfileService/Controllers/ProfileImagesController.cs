﻿using Amazon.S3;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfileService.Common.Services;

namespace ProfileService.Controllers;

[Authorize]
[ApiController]
[Route("api/v{version:apiVersion}/profiles/{userId:guid}/image")]
[ApiVersion("1.0")]
public class ProfileImagesController(IProfileImageService imageService) : ControllerBase
{
    private readonly IProfileImageService _imageService = imageService;

    [Authorize]
    [HttpPost("upload")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadProfileImage([FromRoute] Guid userId, IFormFile profileImage)
    {
        var result = await _imageService.UploadProfileImageAsync(profileImage, userId);

        return result switch
        {
            true => StatusCode(201, new { message = "Profile image uploaded successfully" }),
            _ => BadRequest()
        };
    }

    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProfileImage([FromRoute] Guid userId)
    {
        try
        {
            var result = await _imageService.GetProfileImageAsync(userId);

            return File(result.ResponseStream, result.Headers.ContentType);
        }
        catch (AmazonS3Exception ex)
        {
            return NotFound("There was an error retrieving the profile image.");
        }
    }


    [Authorize]
    [HttpDelete("remove")]
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