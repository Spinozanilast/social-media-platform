namespace IdentityService.Contracts.Api.Registration;

public class RegistrationResponse
{
    public bool IsSuccessfulRegistration { get; set; }
    public IEnumerable<string>? Errors { get; set; }
}