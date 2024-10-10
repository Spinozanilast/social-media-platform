namespace IdentityService.Entities.Tokens;

public class Token
{
    public required string TokenValue { get; set; }
    public DateTime ExpiryDate { get; set; }
}