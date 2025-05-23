using AuthorizationService.Entities.OAuthInfos;

namespace AuthorizationService.Contracts.Users;

public record UserDto(Guid Id, string UserName, string? FirstName, string LastName, string Email, GithubInfo GithubInfo, string? PhoneNumber);