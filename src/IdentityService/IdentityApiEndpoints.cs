using Microsoft.AspNetCore.StaticFiles;

namespace IdentityService;

public static class ApiEndpoints
{
    public static class AccountEndpoints
    {
        public const string Base = "accounts";
        public const string Register = $"{Base}/register";



    }
}
