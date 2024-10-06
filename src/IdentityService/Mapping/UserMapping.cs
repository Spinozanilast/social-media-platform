using IdentityService.Contracts;
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

    public static LoginResponse ToLoginResponse(this User user, IList<string> roles)
    {
        return new LoginResponse(
            Id: user.Id,
            FirstName: user.FirstName,
            LastName: user.LastName,
            UserName: user.UserName!,
            Roles: roles.ToArray()
        );
    }

    public static UserToGet ToUserGetModel(this User user)
    {
        return new UserToGet(
            Id: user.Id,
            Email: user.Email!,
            LastName: user.LastName,
            FirstName: user.FirstName,
            PhoneNumber: user.PhoneNumber,
            Username: user.UserName!
        );
    }

    public static User UpdateUser(this User user, UserUpdateDto userUpdateDto)
    {
        user.FirstName = userUpdateDto.FirstName;
        user.LastName = userUpdateDto.LastName;
        user.UserName = userUpdateDto.Username;
        user.PhoneNumber = userUpdateDto.PhoneNumber;
        user.Email = userUpdateDto.Email;

        return user;
    }
}