namespace IdentityService.Contracts;

public readonly struct DefaultResponse(
    bool isSuccess,
    IEnumerable<string>? errorFields,
    IEnumerable<string>? errors)
{
    public bool IsSuccess { get; } = isSuccess;
    public IEnumerable<string>? ErrorFields { get; } = errorFields;
    public IEnumerable<string>? Errors { get; } = errors;

    public static DefaultResponse DefaultSuccessResponse() => new DefaultResponse(true, null, null);
}