namespace IdentityService.Entities;

public class Token
{
    public required string TokenValue { get; set; }
    public DateTime ExpiryDate { get; set; }
}