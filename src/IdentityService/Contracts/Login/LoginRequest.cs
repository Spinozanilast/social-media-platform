using System.ComponentModel.DataAnnotations;

namespace IdentityService.Contracts.Authentication;

public class LoginRequest
{
    [Required] public string Email { get; init; }
    [Required] public string Password { get; init; }
}