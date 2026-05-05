using SocialMedia.Infrastructure.Domain.Entities.Security;
namespace SocialMedia.Infrastructure.Domain.Entities.Business.Profiles;
public sealed class Block
{
    public Guid BlockerId { set; get; }
    public Guid BlockedId { set; get; }
    public User Blocker { set; get; }
    public User Blocked { set; get; }
}
