using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using ProfileService.Common.Services;
using ProfileService.Data;

namespace ProfileService.Services;

public class ProfileImageService(
    IAmazonS3 s3Client,
    ILogger<ProfileImageService> logger,
    IOptions<ProfileImageStorageConfig> storageOptions)
    : IProfileImageService
{
    private readonly IAmazonS3 _s3Client = s3Client;
    private readonly ILogger<ProfileImageService> _logger = logger;

    private readonly string _bucketName = storageOptions.Value.BucketName;

    public async Task<GetObjectResponse> GetProfileImageAsync(Guid id)
    {
        var getImageRequest = new GetObjectRequest
        {
            BucketName = _bucketName,
            Key = GetImageKey(id),
        };

        return await _s3Client.GetObjectAsync(getImageRequest);
    }

    public async Task<bool> UploadProfileImageAsync(IFormFile image, Guid id)
    {
        try
        {
            if (image is null)
            {
                throw new ArgumentNullException(nameof(image), "Image was not found");
            }

            var putImageRequest = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = GetImageKey(id),
                InputStream = image.OpenReadStream(),
                ContentType = image.ContentType,
                Metadata =
                {
                    ["x-amz-meta-original-file-name"] = image.FileName,
                    ["x-amz-meta-original-file-extension"] = Path.GetExtension(image.FileName),
                }
            };

            var response = await _s3Client.PutObjectAsync(putImageRequest);
            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                return true;
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, nameof(UploadProfileImageAsync));
        }

        return false;
    }

    public async Task<bool> RemoveProfileImageAsync(Guid id)
    {
        try
        {
            var deleteObjectRequest = new DeleteObjectRequest()
            {
                BucketName = _bucketName,
                Key = GetImageKey(id)
            };

            var response = await _s3Client.DeleteObjectAsync(deleteObjectRequest);
            if (response.HttpStatusCode == HttpStatusCode.NoContent)
            {
                return true;
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, nameof(UploadProfileImageAsync));
        }

        return false;
    }

    private string GetImageKey(Guid id) => $"profile-picture/{id}";
}