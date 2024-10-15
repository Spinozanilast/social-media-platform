namespace IdentityService;

public static class IdentityApiEndpoints
{
    private const string Base = "user";

    public static class AccountEndpoints
    {
        public const string Register = $"{Base}/register";
        public const string Login = $"{Base}/login";
        public const string SignOut = $"{Base}/signout";

        public const string GetUserByIdOrUsername = $"{Base}s/" + "{idOrUsername}";
        public const string GetAll = $"{Base}s/get";

        public const string UpdateUser = $"{Base}/" + "{id}/update";
    }

    public static class ProfileImagesEndpoints
    {
        private const string Base = $"{IdentityApiEndpoints.Base}/" + "{idOrUsername}" + "/profile-image";

        public const string Upload = $"{Base}/upload";
        public const string Remove = $"{Base}/remove";
        public const string Get = Base;
    }

    public static class TokensEndpoints
    {
        private const string Base = $"{IdentityApiEndpoints.Base}/" + "{idOrUsername}";

        public const string GetRefreshTokens = $"{IdentityApiEndpoints.Base}/" + "{userId}" + "/tokens";
        public const string RefreshToken = $"{Base}/refresh-token";
        public const string RevokeToken = $"{Base}/revoke-token";
    }
}