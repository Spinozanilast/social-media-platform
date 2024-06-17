namespace Domain;

public class User
{
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required ProfileMeta ProfileMeta { get; set; }
    public string? Patronymic { get; set; }
    public string? Phone { get; set; }
    public Role Role { get; set; }
    public DateOnly BirthdayDate { get; set; }
    public string? Country { get; set; }
    public required string Nickname { get; set; }
    public required string Password { get; set; }
    public List<User> Friends { get; set; } = new();
    public List<Story> Stories { get; set; } = new();
    public List<GroupStory> GroupStories { get; set; } = new();
}
