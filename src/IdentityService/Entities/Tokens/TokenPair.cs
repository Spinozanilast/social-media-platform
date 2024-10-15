namespace IdentityService.Entities.Tokens;

public record TokenPair(Token JwtToken, RefreshToken RefreshToken);