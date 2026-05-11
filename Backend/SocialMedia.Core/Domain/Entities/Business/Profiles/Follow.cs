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
