using SocialMedia.Infrastructure.Domain.Entities.Base;

namespace SocialMedia.Infrastructure.Domain.Entities.Business.Posts;
public sealed class Image : BaseEntity<Guid>
{
    public byte[] Data { get; set; }
    public string? FileName { get; set; }
    public string? ContentType { get; set; }

    public Guid? PostId { get; set; }
    public Post? Post { get; set; }
}