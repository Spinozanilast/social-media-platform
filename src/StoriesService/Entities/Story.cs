namespace StoriesService.Entities;

public class Story
{
    public int Id { get; set; }
    public Guid AuthorId { get; set; }
    
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    
    public List<string> Tags { get; set; } = [];
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    public int? OriginalPostId { get; set; }
    
    public bool IsShared { get; set; } = false;
    public bool IsEdited { get; set; } = false;

    public Story OriginalPost { get; set; }
}