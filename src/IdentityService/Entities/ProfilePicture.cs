using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityService.Entities;

public class ProfilePicture
{
    [Required] public required Stream ImageData { get; set; }

    [Required] [StringLength(10)] public required string ContentType { get; set; }
}