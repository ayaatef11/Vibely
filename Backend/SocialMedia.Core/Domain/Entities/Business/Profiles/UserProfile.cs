using SocialMedia.Core.Domain.Entities.Business.Stories;

namespace SocialMedia.Core.Domain.Entities.Business.Profiles;
public class UserProfile : BaseEntity<Guid>
{
    public int PostCount { set; get; }
    public int FollowerCount { set; get; }
    public int FollowingCount { set; get; }
    public string? Bio { get; set; } = string.Empty;
    public string FullName { set; get; } = string.Empty;
    public string UserName { set; get; } = string.Empty;
    public string? Website { set; get; } = string.Empty;
    public string? Location { set; get; } = string.Empty;

    // images
    public string? ProfileImage { get; set; }
    public string? BackgroundImage { get; set; }
    public string? ProfileImageContentType { get; set; }
    public string? BackgroundImageContentType { get; set; }
    public Guid UserId { set; get; }
    public User User { set; get; }
    public ICollection<Notification> Senders { set; get; } = new List<Notification>();
    public ICollection<Notification> Receivers { set; get; } = new List<Notification>();
    public ICollection<Follow> Following { set; get; } = new List<Follow>();
    public ICollection<Follow> Followers { set; get; } = new List<Follow>();
    public ICollection<Post> Posts { set; get; } = new List<Post>();
    public ICollection<Share> Shares { set; get; } = new List<Share>();
    public ICollection<Story> Stories { set; get; } = new List<Story>();
    public ICollection<StoryComment> storyComments { set; get; } = new List<StoryComment>();
    public ICollection<Comment> Comments { set; get; } = new List<Comment>();
    public ICollection<PostLikes> Reacts { set; get; } = new List<PostLikes>();
    public ICollection<CommentLikes> CommentLikes { set; get; } = new List<CommentLikes>();

}