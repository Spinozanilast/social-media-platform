using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProfileService.Entities;

public sealed class Country
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }

    public required string Name { get; init; }
    public required string IsoCode { get; init; }

    [JsonIgnore]
    public ICollection<Profile> Profiles { get; set; }
}