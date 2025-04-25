using System.ComponentModel.DataAnnotations;
using AuthorizationService.Entities.Tokens;
using Microsoft.AspNetCore.Identity;

namespace AuthorizationService.Entities;

public class User : IdentityUser<Guid>
{
    [Required] [StringLength(40)] public required string FirstName { get; set; }
    [Required] [StringLength(40)] public required string LastName { get; set; }

    public List<RefreshToken> RefreshTokens { get; } = new();
}