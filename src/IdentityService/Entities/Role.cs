using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Entities;

public class Role : IdentityRole<Guid>
{
    [StringLength(100)] public string? Description { get; private set; } = string.Empty;

    public Role(string name) : base(name)
    {
    }

    public Role(string name, string description) : base(name) => Description = Description;
}