using AuthorizationService.Contracts.Users;
using AuthorizationService.Entities;

namespace AuthorizationService.Common.Mappers;

public static class UserMapping
{
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
}