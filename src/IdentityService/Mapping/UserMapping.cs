using IdentityService.Contracts.Registration;
using IdentityService.Entities;

namespace IdentityService.Mapping;

public static class UserMapping
{
    public static User ToUserWithoutHashPassword(this UserForRegistration userForRegistration)
    {
        return new User
        {
            FirstName = userForRegistration.FirstName,
            LastName = userForRegistration.LastName,
            Email = userForRegistration.Email,
            UserName = userForRegistration.Username,
        };
    }
}

