namespace Domain;

public class StoryMeta
{
    public required string Header { get; set; }
    public Guid[] Creators { get; set; }
    public DateOnly CreationTimeStamp { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
    public DateTime LastEditTimeStamp { get; set; } = DateTime.UtcNow;
    public string[]? Tags { get; set; }
}
