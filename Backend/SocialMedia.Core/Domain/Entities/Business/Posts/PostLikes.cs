using SocialMedia.Core.Domain.Entities.Business.Profiles; 
using SocialMedia.Infrastructure.Domain.Enums;

namespace SocialMedia.Infrastructure.Domain.Entities.Business.Posts;
public sealed class PostLikes : BaseEntity<Guid>
{
    public ReactionType ReactionType { set; get; }

    public Post Post { set; get; }
    public Guid PostId { set; get; }
    public Guid ProfileId { set; get; }
    public UserProfile Profile { set; get; }
}
