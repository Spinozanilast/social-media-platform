using IdentityService.Contracts.Registration;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Mapping;

public static class ResultErrorMapping
{
    public static RegistrationResponse ToRegistrationResponse(this IdentityResult identityResult)
    {
        var response = new RegistrationResponse(
            IsSuccessfulRegistration: false,
            ErrorFields: identityResult.Errors.Select(error => ExtractFieldName(error.Description)),
            Errors: identityResult.Errors.Select(error => error.Description)
        );
        return response;
    }

    private static string ExtractFieldName(string errorDescription) =>
        errorDescription.Split(' ').FirstOrDefault() ?? string.Empty;
}