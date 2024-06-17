namespace Domain;

public class GroupStory : AuditableEntity
{
    public Guid Id { get; set; }
    public required StoryMeta StoryMeta { get; set; }
}
