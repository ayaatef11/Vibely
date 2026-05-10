namespace SocialMedia.Core.Domain.Entities.Business.Posts;
public sealed class Reels : BaseEntity<Guid>
{
    public string? FileName { get; set; }
    public DateTime UploadAt { get; set; }
    public string? ContentType { get; set; }
    public byte[] Data { get; set; } = Array.Empty<byte>();

    public Guid? UserId { get; set; }
    public User? User { get; set; }
}
