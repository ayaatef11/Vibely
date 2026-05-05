using SocialMedia.Core.Domain.Entities.Business.Profiles;
using SocialMedia.Infrastructure.Domain.Entities.Base;
using SocialMedia.Infrastructure.Domain.Entities.Security;

namespace SocialMedia.Infrastructure.Domain.Entities.Business.Posts;
public sealed class Share : BaseEntity<Guid>
{
    public DateTime CreatedAt { set; get; }

    public Post Post { set; get; }
    public Guid PostId { set; get; }
    public Guid ProfileId { set; get; }
    public string? ShareToken { set; get; }
    public UserProfile Profile { set; get; }
}
