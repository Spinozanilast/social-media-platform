using IdentityService.Contracts.Login;
using IdentityService.Contracts.Registration;
using IdentityService.Entities;

namespace IdentityService.Mapping;

public static class UserMapping
{
    public static User ToUserWithoutHashPassword(this UserForRegistration userForRegistration)
    {
        return new User
        {
            FirstName = userForRegistration.FirstName!,
            LastName = userForRegistration.LastName!,
            Email = userForRegistration.Email,
            UserName = userForRegistration.Username,
        };
    }

    public static LoginResponse ToLoginResponse(this User user, string tokenString)
    {
        return new LoginResponse(
            Id: user.Id,
            FirstName: user.FirstName,
            LastName: user.LastName,
            UserName: user.UserName ?? string.Empty,
            Token: tokenString
        );
    }
}