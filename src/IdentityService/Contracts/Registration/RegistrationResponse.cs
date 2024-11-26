namespace IdentityService.Contracts.Registration;

public readonly record struct RegistrationResponse(
    Guid UserId,
    IEnumerable<string>? ErrorFields,
    IEnumerable<string>? Errors)
{
    public static RegistrationResponse CreateErrorResponse(IEnumerable<string>? errorFields,
        IEnumerable<string>? errors)
    {
        return new RegistrationResponse(Guid.Empty, errorFields, errors);
    }

    public static RegistrationResponse CreateSuccessResponse(Guid userId)
    {
        return new RegistrationResponse(userId, null, null);
    }
}