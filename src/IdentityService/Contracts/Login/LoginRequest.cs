﻿using System.ComponentModel.DataAnnotations;

namespace IdentityService.Contracts.Login;

public class LoginRequest
{
    [Required] public required string Email { get; init; }
    [Required] public required string Password { get; init; }
    [Required] public required bool RememberMe { get; init; }
}