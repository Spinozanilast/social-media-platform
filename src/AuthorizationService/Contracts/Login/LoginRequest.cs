using System.ComponentModel.DataAnnotations;

namespace AuthorizationService.Contracts.Login;

public class LoginRequest
{
    [Required] public required string Email { get; init; }
    [Required] public required string Password { get; init; }
    [Required] public required bool RememberMe { get; init; }
}