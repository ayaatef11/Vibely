using SocialMedia.Infrastructure.Domain.Enums;

namespace SocialMedia.Core.Domain.DTOs.Requests.Like;
public class LikeDTO
{
    public Guid PostId { set; get; }
    public Guid ProfileId { set; get; }
    public ReactionType React { set; get; }
}
