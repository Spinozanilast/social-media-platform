using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProfileService.Entities;

public sealed class Country
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }

    public required string Name { get; init; }
    public required string IsoCode { get; init; }

    public ICollection<Profile> Profiles { get; set; }
}