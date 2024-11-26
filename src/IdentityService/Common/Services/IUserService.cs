using IdentityService.Contracts;
using IdentityService.Contracts.Login;
using IdentityService.Contracts.Registration;
using IdentityService.Helpers;

namespace IdentityService.Common.Services;

public interface IUserService
{
    ValueTask<Result<RegistrationResponse>> RegisterUserAsync(UserForRegistration userForRegistration);
    ValueTask SignOut(HttpRequest request, HttpResponse response);
    Task<Result<LoginResponse>> LoginUserAsyncWithCookiesSet(LoginRequest loginRequest, HttpResponse response);
    Task<Result<UserToGet>> GetUserByIdOrUsernameAsync(string idOrUsername);
    Task<Result<IEnumerable<UserToGet>>> GetAllUserAsync();
    Task<Result<string>> UpdateUserIdentityAsync(string id, UserUpdateDto updatedUser);
}