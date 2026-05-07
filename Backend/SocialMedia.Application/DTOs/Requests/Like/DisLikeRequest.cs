namespace SocialMedia.Core.Domain.DTOs.Requests.Like;
public class DisLikeRequest
{
    public Guid PostId { set; get; }
    public Guid ProfileId { set; get; }
}
