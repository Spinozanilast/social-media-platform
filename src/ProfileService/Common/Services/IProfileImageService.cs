using ProfileService.Models;

namespace ProfileService.Common.Services;

public interface IProfileImageService
{
    Task<ProfilePicture?> GetProfileImageAsync(Guid id);
    Task<bool> UploadProfileImageAsync(Image image, Guid id);
    Task<bool> RemoveProfileImageAsync(Guid id);
}