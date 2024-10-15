using IdentityService.Contracts;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Mapping;

public static class ResultErrorMapping
{
    public static DefaultResponse ToDefaultErrorResponse(this IdentityResult identityResult)
    {
        return new DefaultResponse(
            isSuccess: false,
            errorFields: identityResult.Errors.Select(error => ExtractFieldName(error.Description)),
            errors: identityResult.Errors.Select(error => error.Description)
        );
    }

    private static string ExtractFieldName(string errorDescription) =>
        errorDescription.Split(' ').FirstOrDefault()?.ToLower() ?? string.Empty;
}