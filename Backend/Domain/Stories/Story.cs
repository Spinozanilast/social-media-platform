namespace Domain;

public class Story : AuditableEntity
{
    public Guid Id { get; set; }
    public required string UserId { get; set; }
    public required StoryMeta RecordMeta { get; set; }
    public Image[]? Images { get; set; }
    public string Content { get; set; } = "";
}
