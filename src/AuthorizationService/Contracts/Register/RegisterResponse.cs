namespace AuthorizationService.Contracts.Register;

public readonly record struct RegisterResponse(Guid UserId, string UserName);