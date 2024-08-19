namespace Authentication.Extensions.Configurations;

public class TokenConfiguration
{
    public required string SecretKey { get; set; }
    public int ExpiryDays { get; set; }
}