using Microsoft.EntityFrameworkCore;

namespace IdentityService.Entities.Tokens;

[Owned]
public class RefreshToken : Token
{
    private bool IsExpired => DateTime.UtcNow >= ExpiryDate;
    public DateTime Created { get; set; }
    public DateTime? Revoked { get; set; }
    public bool IsActive => Revoked == null && !IsExpired;
    
}