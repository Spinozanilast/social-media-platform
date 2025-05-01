using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using ProfileService.Data.Configurations;

namespace ProfileService.Entities;

[EntityTypeConfiguration(typeof(CountriesConfiguration))]
public sealed class Country
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }

    public required string Name { get; init; }
    public required string IsoCode { get; init; }

    [JsonIgnore] public ICollection<Profile> Profiles { get; set; }
}