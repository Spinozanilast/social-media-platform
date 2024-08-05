using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Entities;

public class Role : IdentityRole<Guid>
{
    [StringLength(100)]
    public string? Description { get; set; } = string.Empty;
}
