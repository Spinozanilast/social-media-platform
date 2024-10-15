using IdentityService.Contracts;
using IdentityService.Contracts.Login;
using IdentityService.Contracts.Registration;
using IdentityService.Utilities;

namespace IdentityService.Services;

public interface IUserService
{
    ValueTask<DefaultResponse> RegisterUserAsync(UserForRegistration userForRegistration);
    ValueTask<DefaultResponse> SignOut(HttpRequest request, HttpResponse response);
    Task<Result<LoginResponse>> LoginUserAsyncWithCookiesSet(LoginRequest loginRequest, HttpResponse response);
    Task<Result<UserToGet>> GetUserByIdOrUsernameAsync(string idOrUsername);
    Task<Result<IEnumerable<UserToGet>>> GetAllUserAsync();
    Task<Result<string>> UpdateUserIdentityAsync(string id, UserUpdateDto updatedUser);
}