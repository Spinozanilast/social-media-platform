namespace Domain;

public class Image
{
    public ImageMeta? ImageMeta { get; set; }
    public required byte[] ImageData { get; set; }
    public Guid Id { get; set; }
}
