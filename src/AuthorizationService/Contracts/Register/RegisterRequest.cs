using System.ComponentModel.DataAnnotations;

namespace AuthorizationService.Contracts.Register;

public class RegisterRequest
{
    [Required] public required string FirstName { get; set; }
    [Required] public required string LastName { get; set; }
    [Required] public required string UserName { get; set; }

    [Required] [EmailAddress] public required string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public required string Password { get; set; }
}