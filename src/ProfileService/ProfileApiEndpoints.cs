namespace ProfileService;

public static class ProfileImagesEndpoints
{
    private const string ProfileImagesBase = $"user/" + "{userId:guid}" + "/profile-image";

    public const string Upload = $"{ProfileImagesBase}/upload";
    public const string Remove = $"{ProfileImagesBase}/remove";
    public const string Get = ProfileImagesBase;
}