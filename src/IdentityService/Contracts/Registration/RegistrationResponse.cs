namespace IdentityService.Contracts.Registration;

public record RegistrationResponse(
    bool IsSuccessfulRegistration,
    IEnumerable<string> ErrorFields,
    IEnumerable<string> Errors
);