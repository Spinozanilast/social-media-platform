using IdentityService.Contracts.Registration;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Mapping;

public static class ResultErrorMapping
{
    public static RegistrationResponse ToDefaultErrorResponse(this IdentityResult identityResult)
    {
        return RegistrationResponse.CreateErrorResponse(
            errorFields: identityResult.Errors.Select(error => ExtractFieldName(error.Description)),
            errors: identityResult.Errors.Select(error => error.Description)
        );
    }

    private static string ExtractFieldName(string errorDescription) =>
        errorDescription.Split(' ').FirstOrDefault()?.ToLower() ?? string.Empty;
}