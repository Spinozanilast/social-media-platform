﻿namespace IdentityService;

public static class IdentityApiEndpoints
{
    private const string Base = "user";

    public static class AccountEndpoints
    {
        public const string Register = $"{Base}/register";
        public const string Login = $"{Base}/login";

        public const string GetUserByIdOrUsername = $"{Base}s/" + "{idOrUsername}";
        public const string GetAll = $"{Base}s/get";

        public const string UpdateUser = $"{Base}/" + "{id}/update";

        public const string Authenticate = $"{Base}/" + "{id}/update";
    }

    public static class ProfileImagesEndpoints
    {
        private const string Base = $"{IdentityApiEndpoints.Base}/" + "{userId}" + "/profile-image";

        public const string Upload = $"{Base}/upload";
        public const string Remove = $"{Base}/remove";
        public const string Get = Base;
    }

    public static class TokensEndpoints
    {
        private const string Base = $"{IdentityApiEndpoints.Base}/" + "{userId}";

        public const string GetRefreshTokens = $"{Base}/tokens";
        public const string RefreshToken = $"{Base}/refresh-token";
    }
}