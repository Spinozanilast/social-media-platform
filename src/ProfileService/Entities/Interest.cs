using System.ComponentModel.DataAnnotations;

namespace ProfileService.Entities;

public class Interest
{
    public int InterestId { get; set; }
    [MaxLength(30)] public required string Name { get; set; }

    public ICollection<Profile> Profiles { get; set; }
}