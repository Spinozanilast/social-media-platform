using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using ProfileService.Data.Configurations;

namespace ProfileService.Entities;

[EntityTypeConfiguration(typeof(ProfilesConfiguration))]
public class Profile(Guid userId)
{
    public Guid UserId { get; init; } = userId;

    [MaxLength(100)] public string About { get; set; } = string.Empty;

    public DateOnly? BirthDate { get; set; }
    public Country? Country { get; set; }

    public List<string> Interests { get; set; } = [];
    public List<string> References { get; set; } = [];
}