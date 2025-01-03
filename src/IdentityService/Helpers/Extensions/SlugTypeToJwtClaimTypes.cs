using System.IdentityModel.Tokens.Jwt;
using IdentityService.Entities.Enums;

namespace IdentityService.Helpers;

public static class SlugTypeToJwtClaimTypes
{
    public static string GetClaimNameFromSlugType(this UserSlugTypes type) => type switch
    {
        UserSlugTypes.UserName => JwtRegisteredClaimNames.UniqueName,
        UserSlugTypes.Guid => "uid",
        _ => throw new ArgumentException("Invalid enum value for type", nameof(type))
    };
}