using IdentityService.Contracts.Register;
using IdentityService.Contracts.Users;
using IdentityService.Entities;

namespace IdentityService.Common.Mappers;

public static class UserMapping
{
    public static User ToUserWithoutHashPassword(this RegisterRequest request)
    {
        return new User
        {
            FirstName = request.FirstName!,
            LastName = request.LastName!,
            Email = request.Email,
            UserName = request.UserName,
        };
    }

    public static UserDto ToUserDto(this User user)
    {
        return new UserDto(
            Id: user.Id,
            Email: user.Email!,
            LastName: user.LastName,
            FirstName: user.FirstName,
            PhoneNumber: user.PhoneNumber,
            UserName: user.UserName!
        );
    }

    public static User UpdateUser(this User user, UserUpdateRequest userUpdateRequest)
    {
        user.FirstName = userUpdateRequest.FirstName;
        user.LastName = userUpdateRequest.LastName;
        user.UserName = userUpdateRequest.UserName;
        user.PhoneNumber = userUpdateRequest.PhoneNumber;
        user.Email = userUpdateRequest.Email;

        return user;
    }
}