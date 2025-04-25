using AuthorizationService.Contracts.Register;
using Microsoft.AspNetCore.Identity;

namespace AuthorizationService.Common.Mappers;

public static class ResultErrorMapper
{
    private static readonly Dictionary<string, string> ErrorsNamesReplacements =
        new()
        {
            { "passwords", "password" }
        };

    public static RegisterErrorsResponse ToDefaultErrorResponse(this IdentityResult identityResult)
    {
        var errors = AggregateIdentityErrors(identityResult);
        return new RegisterErrorsResponse(errors);
    }

    private static Dictionary<string, List<string>> AggregateIdentityErrors(IdentityResult identityResult)
    {
        var fieldsErrors = new Dictionary<string, List<string>>();

        foreach (var error in identityResult.Errors)
        {
            var fieldName = ExtractFieldName(error.Description);

            if (ErrorsNamesReplacements.TryGetValue(fieldName, out var replacement))
            {
                fieldName = replacement;
            }

            if (!fieldsErrors.TryGetValue(fieldName, out var value))
            {
                fieldsErrors.Add(fieldName, [error.Description]);
            }
            else
            {
                value.Add(error.Description);
            }
        }

        return fieldsErrors;
    }

    private static string ExtractFieldName(string errorDescription) =>
        errorDescription.Split(' ').FirstOrDefault()?.ToLower() ?? string.Empty;
}