using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using TypeGen.Core.TypeAnnotations;

namespace IdentityService.Entities;

[ExportTsClass(OutputDir = "../web-app/app/models/shared")]
public class User : IdentityUser<Guid>
{
    [Required] [StringLength(40)] public required string FirstName { get; set; }
    [Required] [StringLength(40)] public required string LastName { get; set; }
}