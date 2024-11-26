namespace IdentityService;

public static class IdentityApiEndpoints
{
    private const string IdentityBase = "user";

    public static class AccountEndpoints
    {
        public const string Register = $"{IdentityBase}/register";
        public const string Login = $"{IdentityBase}/login";
        public const string SignOut = $"{IdentityBase}/signout";

        public const string GetUserByIdOrUsername = $"{IdentityBase}/" + "{idOrUsername}";
        public const string GetAll = $"{IdentityBase}s/get";

        public const string UpdateUser = $"{IdentityBase}/" + "{id}/update";
    }

    public static class TokensEndpoints
    {
        private const string TokensBase = $"{IdentityBase}/" + "{idOrUsername}";

        public const string GetRefreshTokens = $"{IdentityBase}/" + "{userId}" + "/tokens";
        public const string RefreshToken = $"{TokensBase}/refresh-token";
        public const string RevokeToken = $"{TokensBase}/revoke-token";
        public const string CheckToken = $"{TokensBase}/check-token";
    }
}