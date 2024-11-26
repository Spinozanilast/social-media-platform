using IdentityService.Common.Services;
using IdentityService.Contracts;
using IdentityService.Contracts.Login;
using IdentityService.Contracts.Registration;
using IdentityService.Entities;
using IdentityService.Helpers;
using IdentityService.Mapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly ICookiesService _cookiesService;

    public UserService(UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService,
        ICookiesService cookiesService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _cookiesService = cookiesService;
    }

    public async ValueTask<Result<RegistrationResponse>> RegisterUserAsync(UserForRegistration userForRegistration)
    {
        var user = userForRegistration.ToUserWithoutHashPassword();
        user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, userForRegistration.Password);

        var result = await _userManager.CreateAsync(user);

        return result.Succeeded
            ? Result<RegistrationResponse>.Success(RegistrationResponse.CreateSuccessResponse(user.Id))
            : Result<RegistrationResponse>.Failure("Request has some issues", result.ToDefaultErrorResponse());
    }

    public async ValueTask SignOut(HttpRequest request, HttpResponse response)
    {
        _cookiesService.ExpireAuthHttpOnlyCookies(request.Cookies, response.Cookies);
        await _signInManager.SignOutAsync();
    }

    private async Task<Result<(LoginResponse, User?)>> LoginUserAsync(LoginRequest loginRequest)
    {
        var user = await _userManager.FindByEmailAsync(loginRequest.Email);

        if (user is null)
        {
            return Result<(LoginResponse, User?)>.Failure("User with such email was not found");
        }

        var loginResult =
            await _signInManager.PasswordSignInAsync(user, loginRequest.Password, loginRequest.RememberMe, false);

        if (!loginResult.Succeeded)
        {
            return Result<(LoginResponse, User?)>.Failure("Password or email is wrong");
        }

        var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
        return Result<(LoginResponse, User?)>.Success((user.ToLoginResponse(rolesList), user));
    }

    public async Task<Result<LoginResponse>> LoginUserAsyncWithCookiesSet(LoginRequest loginRequest,
        HttpResponse response)
    {
        Result<(LoginResponse loginResponse, User? user)> loginResultWithUser = await LoginUserAsync(loginRequest);

        if (!loginResultWithUser.IsSuccess || loginResultWithUser.Value.user is null)
            return Result<LoginResponse>.Failure(loginResultWithUser.Error);

        var user = loginResultWithUser.Value.user;
        var jwtToken = await _tokenService.GenerateJwtToken(user);
        var refreshToken = await _tokenService.GenerateRefreshTokenWithSave(user);

        _tokenService.SetTokensInCookies(response, jwtToken, refreshToken);

        return Result<LoginResponse>.Success(loginResultWithUser.Value.loginResponse);
    }

    public async Task<Result<UserToGet>> GetUserByIdOrUsernameAsync(string idOrUsername)
    {
        var user = await _userManager.FindUserByUserNameThenId(idOrUsername);

        return user is null
            ? Result<UserToGet>.Failure("User with such id or username was not found")
            : Result<UserToGet>.Success(user.ToUserGetModel());
    }

    public async Task<Result<IEnumerable<UserToGet>>> GetAllUserAsync()
    {
        var users = await _userManager.Users.Select(u => u.ToUserGetModel()).ToArrayAsync();

        return users.Length == 0
            ? Result<IEnumerable<UserToGet>>.Failure("No users found")
            : Result<IEnumerable<UserToGet>>.Success(users);
    }

    public async Task<Result<string>> UpdateUserIdentityAsync(string id, UserUpdateDto updatedUser)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user is null)
        {
            return Result<string>.Failure("User with such id was not found");
        }

        var result = await _userManager.UpdateAsync(user.UpdateUser(updatedUser));

        return result.Succeeded
            ? Result<string>.Success()
            : Result<string>.Failure(result.Errors.Select(e => e.Description).FirstOrDefault());
    }
}