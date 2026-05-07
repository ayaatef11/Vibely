using SocialMedia.Infrastructure.Domain.Enums;

namespace SocialMedia.Core.Domain.DTOs.Requests.Comment
{
    public class DisLikeCommentRequest
    {
        public Guid UserId { set; get; }
        public Guid PostId { set; get; }
        public Guid CommentId { set; get; }
        public ReactionType ReactionType { set; get; }
    }
}
