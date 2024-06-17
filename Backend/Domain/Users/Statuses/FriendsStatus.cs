namespace Domain;

public class FriendsStatus
{
    public required string StatusName { get; set; }
    public IEnumerable<Guid> FriendsWithStatus { get; set; } = [];
}
