namespace SocialMedia.Application.DTOs.Responses;
public class CommentResponse
{
    public Guid Id {  get; set; }
    public string Text { set; get; }
    public long ReactCount { set; get; }
    public DateTime AddedAt { set; get; }
    public Guid PostId { set; get; }
    public Guid ProfileId { set; get; }
    public string UserName { set; get; } = string.Empty;

}

