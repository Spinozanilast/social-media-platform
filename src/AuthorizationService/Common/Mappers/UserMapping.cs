using AuthorizationService.Contracts.Users;
using AuthorizationService.Entities;

namespace AuthorizationService.Common.Mappers;

public static class UserMapping
{
    public static UserDto ToUserDto(this User user)
    {
        return new UserDto(
            Id: user.Id,
            UserName: user.UserName!,
            FirstName: user.FirstName,
            LastName: user.LastName,
            Email: user.Email!,
            GithubInfo: user.GithubInfo,
            PhoneNumber: user.PhoneNumber
        );
    }
}