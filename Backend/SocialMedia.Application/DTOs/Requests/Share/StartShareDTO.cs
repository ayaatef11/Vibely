namespace SocialMedia.Core.Domain.DTOs.Requests.Share;
public class StartShareDTO
{
    public Guid ProfileId { set; get; }
    public Guid PostId { set; get; }
}
