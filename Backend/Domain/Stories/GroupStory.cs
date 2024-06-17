namespace Domain;

public class GroupStory
{
    public required List<Guid>? EditorsUserGuids { get; set; }
    public required List<Guid> FullAccessUsersGuids { get; set; } = new List<Guid>();
    public required Story Story { get; set; }
}
