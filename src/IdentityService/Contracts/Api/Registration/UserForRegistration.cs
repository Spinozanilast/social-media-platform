using System.ComponentModel.DataAnnotations;

namespace IdentityService.Contracts.Api.Registration;

public class UserForRegistration
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Username { get; set; }

    [EmailAddress]
    public required string Email { get; set; }
    
    [DataType(DataType.Password)]
    public required string Password { get; set; }

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match")]
    public required string ConfirmPassword { get; set; }
}