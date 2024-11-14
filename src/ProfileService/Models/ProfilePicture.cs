using System.ComponentModel.DataAnnotations;

namespace ProfileService.Models;

public class ProfilePicture
{
    [Required] public required Stream ImageData { get; set; }

    [Required] [StringLength(10)] public required string ContentType { get; set; }
}