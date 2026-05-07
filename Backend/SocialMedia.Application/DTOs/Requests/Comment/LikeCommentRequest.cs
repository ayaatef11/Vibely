using SocialMedia.Infrastructure.Domain.Enums;

namespace SocialMedia.Core.Domain.DTOs.Requests.Comment;
public class LikeCommentRequest
{
    public Guid ProfileId { set; get; }
    public Guid PostId { set; get; }
    public Guid CommentId { set; get; }
    public ReactionType ReactionType { set; get; }
}