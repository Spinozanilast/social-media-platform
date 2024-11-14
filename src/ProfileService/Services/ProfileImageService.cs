using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using ProfileService.Common.Services;
using ProfileService.Data;
using ProfileService.Models;

namespace ProfileService.Services;

public class ProfileImageService : IProfileImageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly ILogger<ProfileImageService> _logger;

    private readonly string _bucketName;

    public ProfileImageService(IAmazonS3 s3Client, ILogger<ProfileImageService> logger,
        IOptions<ProfileImageStorageConfig> storageOptions)
    {
        _s3Client = s3Client;
        _logger = logger;

        _bucketName = storageOptions.Value.BucketName;
    }

    public async Task<ProfilePicture?> GetProfileImageAsync(Guid id)
    {
        try
        {
            var getImageRequest = new GetObjectRequest()
            {
                BucketName = _bucketName,
                Key = GetImageKey(id)
            };

            var response = await _s3Client.GetObjectAsync(getImageRequest);

            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                return new ProfilePicture
                {
                    ImageData = response.ResponseStream,
                    ContentType = response.Headers.ContentType
                };
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, nameof(UploadProfileImageAsync));
        }

        return default;
    }

    public async Task<bool> UploadProfileImageAsync(Image image, Guid id)
    {
        try
        {
            if (image is null)
            {
                throw new ArgumentNullException(nameof(image), "Image was not found");
            }

            var putImageRequest = new PutObjectRequest()
            {
                BucketName = _bucketName,
                Key = GetImageKey(id),
                InputStream = image.FormFile.OpenReadStream(),
                ContentType = image.FormFile.ContentType,
                Metadata =
                {
                    ["x-amz-meta-original-file-name"] = image.FormFile.FileName,
                    ["x-amz-meta-original-file-extension"] = Path.GetExtension(image.FormFile.FileName),
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

    private string GetImageKey(Guid id) => $"profile-picture/{id}";
}