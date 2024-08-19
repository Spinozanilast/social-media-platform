namespace IdentityService;

public static class ApiEndpoints
{
    public static class AccountEndpoints
    {
        public const string Base = "user";

        public const string SignUp = $"{Base}/register";
        public const string Signin = $"{Base}/login";

        public const string GetUserByIdOrUsername = $"{Base}s/" + "{idOrUsername}";
        public const string GetAll = $"{Base}s/get";

        public const string UpdateUser = $"{Base}/" + "{id:guid}/update";
    }
}