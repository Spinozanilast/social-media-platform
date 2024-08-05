namespace IdentityService.Contracts.Login;

public record LoginResponse(Guid Id, string FirstName, string LastName, string UserName, string Token);