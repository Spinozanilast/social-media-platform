namespace IdentityService.Entities.Tokens;

public record TokenPair(Token JwtToken, Token RefreshToken);