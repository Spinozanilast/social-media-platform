using IdentityService.Contracts;
using IdentityService.Contracts.Login;
using IdentityService.Contracts.Registration;
using IdentityService.Entities.Enums;
using IdentityService.Helpers;

namespace IdentityService.Common.Services;

public interface IUserService
{
    ValueTask<Result<RegistrationResponse>> RegisterUserAsync(UserForRegistration userForRegistration);
    ValueTask SignOut(HttpRequest request, HttpResponse response);
    ValueTask<Result<UserToGet>> GetUserByIdOrUsernameAsync(string idOrUsername);

    ValueTask<Result<(UserToGet user, UserSlugTypes userSlugType)>> GetUserByIdOrUsernameWithSlugTypeAsync(
        string idOrUsername);

    Task<Result<LoginResponse>> LoginUserAsyncWithCookiesSetAsync(LoginRequest loginRequest, HttpResponse response);
    Task<Result<IEnumerable<UserToGet>>> GetAllUserAsync();
    Task<Result<string>> UpdateUserIdentityAsync(string id, UserUpdateDto updatedUser);
}