using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using TypeGen.Core.TypeAnnotations;

namespace IdentityService.Entities;

[ExportTsClass(OutputDir = "../web-app/app/models/shared")]
public class User : IdentityUser<Guid>
{
    public User()
    {
    }

    public User(string userName) : base(userName)
    {
    }

    [Required]
    [StringLength(40)]
    public string FirstName { get; set; }
    [Required]
    [StringLength(40)]
    public string LastName { get; set; }
    
    public ProfilePicture? ProfilePicture { get; set; }
}
