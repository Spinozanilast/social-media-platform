namespace IdentityService.Entities.Tokens;

public record TokenPair(Token AccessToken, RefreshToken RefreshToken);