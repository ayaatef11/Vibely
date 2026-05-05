namespace SocialMedia.Core.Domain.DTOs.Requests.Comment;
public class AddCommentDTO
{
    public string Text { get; set; }
    public Guid ProfileId { set; get; }
    public Guid PostId { set; get; }
}
