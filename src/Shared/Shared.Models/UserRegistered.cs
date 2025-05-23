namespace Shared.Models;

public record UserRegistered(Guid UserId, ICollection<string>? References);