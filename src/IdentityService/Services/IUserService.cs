using IdentityService.Contracts;
using IdentityService.Contracts.Login;
using IdentityService.Contracts.Registration;
using IdentityService.Entities;
using IdentityService.Utilities;

namespace IdentityService.Services;

public interface IUserService
{
    Task<DefaultResponse> RegisterUserAsync(UserForRegistration userForRegistration);
    Task<Result<LoginResponse>> LoginUserAsyncWithCookiesSet(LoginRequest loginRequest, HttpResponse response);
    Task<Result<UserToGet>> GetUserByIdOrUsernameAsync(string idOrUsername);
    Task<Result<IEnumerable<UserToGet>>> GetAllUserAsync();
    Task<Result<string>> UpdateUserIdentityAsync(string id, UserUpdateDto updatedUser);
}