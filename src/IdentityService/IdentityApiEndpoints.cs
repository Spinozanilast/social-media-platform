namespace IdentityService;

public static class IdentityApiEndpoints
{
    private const string Base = "user";

    public static class AccountEndpoints
    {
        public const string Register = $"{Base}/register";
        public const string Login = $"{Base}/login";

        public const string GetUserByIdOrUsername = $"{Base}s/" + "{idOrUsername}";
        public const string GetAll = $"{Base}s/get";

        public const string UpdateUser = $"{Base}/" + "{id:guid}/update";
    }

    public static class ProfileImagesEndpoints
    {
        private const string Base = $"{IdentityApiEndpoints.Base}/" + "userId:guid" + "/profile-image";

        public const string Upload = $"{Base}/upload";
        public const string Remove = $"{Base}/remove";
        public const string Get = Base;
    }
}