namespace SocialMedia.Core.Domain.DTOs.Requests.Block
{
    public class BlockRequest
    {
        public Guid BlockerId { set; get; }
        public Guid BlockedId { set; get; }
    }
}
