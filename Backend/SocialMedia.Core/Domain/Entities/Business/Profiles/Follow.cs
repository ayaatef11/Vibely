using SocialMedia.Core.Domain.Entities.Business.Profiles;
using SocialMedia.Infrastructure.Domain.Entities.Base;
using SocialMedia.Infrastructure.Domain.Entities.Security;
using SocialMedia.Infrastructure.Domain.Enums;

namespace SocialMedia.Infrastructure.Domain.Entities.Business.Profiles;
public sealed class Follow : BaseEntity<Guid>
{
    public FollowState FollowState { set; get; }
    public Guid? FollowerId { set; get; }
    public Guid? FollowingId { set; get; }
    #region RelationShip
    public UserProfile? Follower { set; get; }
    public UserProfile? Following { set; get; }
    #endregion
}
