using System.ComponentModel.DataAnnotations;

namespace IdentityService.Contracts.Registration;

public class UserForRegistration
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Username { get; set; }

    [EmailAddress]
    public required string Email { get; set; }
    
    [DataType(DataType.Password)]
    public required string Password { get; set; }
}