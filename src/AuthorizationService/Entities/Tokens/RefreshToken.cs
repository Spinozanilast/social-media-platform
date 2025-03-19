using Microsoft.EntityFrameworkCore;

namespace IdentityService.Entities.Tokens;

[Owned]
public class RefreshToken : Token
{
    private bool IsExpired => DateTime.UtcNow >= Expires;
    public bool IsActive => Revoked == null && !IsExpired;
    public DateTime CreatedAt { get; set; }
    public DateTime? Revoked { get; set; }
    public string DeviceId { get; set; }
    public string DeviceName { get; set; }
    public string IpAddress { get; set; }
}