namespace SocialMedia.Core.Domain.DTOs.Requests.Share;
public class RevokeShareDTO
{
    public Guid Id { set; get; }
    public Guid ProfileId { set; get; }
    public Guid PostId { set; get; }
}
