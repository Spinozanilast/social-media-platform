using Amazon.S3.Model;

namespace ProfileService.Common.Services;

public interface IProfileImageService
{
    Task<GetObjectResponse> GetProfileImageAsync(Guid id);
    Task<bool> UploadProfileImageAsync(IFormFile image, Guid id);
    Task<bool> RemoveProfileImageAsync(Guid id);
}