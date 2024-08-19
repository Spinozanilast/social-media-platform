using IdentityService.Entities;

namespace IdentityService.Services;

public interface IProfileImageService
{
    Task<ProfilePicture?> GetProfileImageAsync(Guid id);
    Task<bool> UploadProfileImageAsync(IFormFile image, Guid id);
    Task<bool> RemoveProfileImageAsync(Guid id);
}