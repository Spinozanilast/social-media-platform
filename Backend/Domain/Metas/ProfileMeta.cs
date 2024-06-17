namespace Domain;

public class ProfileMeta
{
    public required Image ProfileImages { get; set; }
    public string AdditionalInfo { get; set; } = "";
    public string[] Refs { get; set; } = [];
    public FriendsStatus[]? FriendsStatuses { get; set; }
}
