using System.ComponentModel.DataAnnotations;

namespace ProfileService.Entities;

public class Profile(Guid userId)
{
    public Guid UserId { get; init; } = userId;

    [MaxLength(100)] public string About { get; set; } = string.Empty;
    [MaxLength(200)] public string? Anything { get; set; }

    public DateOnly? BirthDate { get; set; }
    public Country? Country { get; set; }

    public List<Interest> Interests { get; set; } = [];
    public List<string> References { get; set; } = [];
}