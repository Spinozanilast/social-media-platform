using System.ComponentModel.DataAnnotations;
using AuthorizationService.Entities.Tokens;
using Microsoft.AspNetCore.Identity;

namespace AuthorizationService.Entities;

public class User : IdentityUser<Guid>
{
    [StringLength(40)] public string FirstName { get; set; } = string.Empty;
    [StringLength(40)] public string LastName { get; set; } = string.Empty;

    public string? GithubId { get; set; }
    public string? AvatarUrl { get; set; }

    public List<RefreshToken> RefreshTokens { get; } = new();
}