namespace AuthorizationService.Entities.Tokens;

public class Token
{
    public required string TokenValue { get; set; }
    public DateTime Expires { get; set; }
}