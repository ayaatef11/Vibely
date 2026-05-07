namespace SocialMedia.Core.Domain.DTOs.Requests.Comment
{
    public class EditCommentRequest
    {
        public Guid Id { set; get; }
        public string Text { get; set; }
        public Guid UserId { set; get; }
        public Guid PostId { set; get; }
    }
}
