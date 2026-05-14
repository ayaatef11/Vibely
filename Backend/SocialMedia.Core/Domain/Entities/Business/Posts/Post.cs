using SocialMedia.Core.Domain.Entities.Business.Profiles;
using SocialMedia.Infrastructure.Domain.Enums;

namespace SocialMedia.Infrastructure.Domain.Entities.Business.Posts;
public sealed class Post : BaseEntity<Guid>
{
    public long ReactsCount { set; get; }
    public long ShareCount { set; get; }
    public long CommentsCount { set; get; }
    public DateTime CreatedAt { set; get; }
    public FeelingState? FeelingState { set; get; }
    public string Title { set; get; } = string.Empty;
    public string? Text { set; get; } = string.Empty;
    public string? MediaUrls {  set; get; }
    public bool IsHidden {  set; get; } = false;
    public string? SaverIds {  set; get; }
    public bool IsReel { set; get; } = false;
    public Guid ProfileId { set; get; }
    public UserProfile Profile { set; get; }
    public ICollection<PostLikes>? Reacts { set; get; } = new List<PostLikes>();
    public ICollection<Share>? Shares { set; get; } = new List<Share>();
    public ICollection<Image>? Images { set; get; } = new List<Image>();
    public ICollection<Video>? Videos { set; get; } = new List<Video>();
    public ICollection<Comment>? Comments { set; get; } = new List<Comment>();
}