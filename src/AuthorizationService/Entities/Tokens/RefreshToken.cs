using Microsoft.EntityFrameworkCore;

namespace AuthorizationService.Entities.Tokens;

[Owned]
public class RefreshToken : Token
{
    private bool IsExpired => DateTime.UtcNow >= Expires;
    public bool IsActive => Revoked == null && !IsExpired;
    public DateTime CreatedAt { get; init; }
    public DateTime? Revoked { get; set; }
    public string DeviceName { get; init; } = string.Empty;
    public string IpAddress { get; init; } = string.Empty;
    public required bool IsLongActive { get; init; }
}