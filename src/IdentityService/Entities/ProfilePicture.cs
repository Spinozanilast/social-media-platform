using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityService.Entities;

public class ProfilePicture
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public required byte[] ImageData { get; set; }

    [Required]
    [StringLength(10)]
    public required string ContentType { get; set; }
}