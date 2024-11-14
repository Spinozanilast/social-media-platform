using System.ComponentModel.DataAnnotations;

namespace ProfileService.Entities;

public class Profile(Guid userId)
{
    public Guid UserId { get; init; } = userId;

    [MaxLength(100)] public string About { get; set; } = string.Empty;
    [MaxLength(200)] public string? Anything { get; set; }

    public DateOnly? BirthDate { get; set; }
    public string? Country { get; set; } = string.Empty;

    private IList<string> Interests { get; } = new List<string>();
    private IList<string> References { get; } = new List<string>();

    public void AddInterest(string interest)
    {
        Interests.Add(interest);
    }

    public void RemoveInterest(string interest)
    {
        Interests.Remove(interest);
    }

    public void AddReference(string reference)
    {
        References.Add(reference);
    }

    public void RemoveReference(string reference)
    {
        Interests.Remove(reference);
    }
}