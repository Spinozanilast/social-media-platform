using IdentityService.Controllers;
using IdentityService.Entities;

namespace IdentityService.Services;

public interface IProfileImageService
{
    Task<ProfilePicture?> GetProfileImageAsync(Guid id);
    Task<bool> UploadProfileImageAsync(Photo image, Guid id);
    Task<bool> RemoveProfileImageAsync(Guid id);
}