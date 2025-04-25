namespace AuthorizationService.Contracts.Register;

public record RegisterErrorsResponse(Dictionary<string, List<string>> Errors);