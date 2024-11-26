using ProfileService.Entities;

namespace ProfileService.Models;

public class ProfileWithStringifiedInterests
{
    public string About { get; set; }
    public string? Anything { get; set; }
    public DateOnly? BirthDate { get; set; }
    public Country? Country { get; set; }
    public string[] Interests { get; set; }
    public string[] Refs { get; set; }
}