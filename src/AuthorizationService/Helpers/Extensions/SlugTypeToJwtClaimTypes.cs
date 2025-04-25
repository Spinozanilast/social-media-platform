using System.IdentityModel.Tokens.Jwt;
using AuthorizationService.Entities.Enums;

namespace AuthorizationService.Helpers.Extensions;

public static class SlugTypeToJwtClaimTypes
{
    public static string GetClaimNameFromSlugType(this UserSlugTypes type) => type switch
    {
        UserSlugTypes.UserName => JwtRegisteredClaimNames.UniqueName,
        UserSlugTypes.Guid => "uid",
        _ => throw new ArgumentException("Invalid enum value for type", nameof(type))
    };
}