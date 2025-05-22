using System.ComponentModel.DataAnnotations;
using AuthorizationService.Data.Configurations;
using AuthorizationService.Entities.OAuthInfos;
using AuthorizationService.Entities.Tokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationService.Entities;

[EntityTypeConfiguration(typeof(UsersConfiguration))]
public class User : IdentityUser<Guid>
{
    [StringLength(40)] public string FirstName { get; set; } = string.Empty;
    [StringLength(40)] public string LastName { get; set; } = string.Empty;

    public GithubInfo GithubInfo { get; set; } = null!;

    public List<RefreshToken> RefreshTokens { get; } = [];
}