namespace IdentityService.Contracts;

public record struct DefaultResponse(
    bool IsSuccess,
    IEnumerable<string>? ErrorFields,
    IEnumerable<string>? Errors
);