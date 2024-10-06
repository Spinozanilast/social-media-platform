using Microsoft.EntityFrameworkCore;

namespace IdentityService.Entities;

[Owned]
public class RefreshToken : Token
{
    public bool IsExpired => DateTime.UtcNow >= ExpiryDate;
    public DateTime Created { get; set; }
    public DateTime? Revoked { get; set; }
    public bool IsActive => Revoked == null && !IsExpired;
    
}