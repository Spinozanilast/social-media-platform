namespace Domain;

public class AuditableEntity
{
    public required string CreatedBy { get; set; }
    public required string LastModifiedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;
    public int UpdatesNumber { get; set; }
}
