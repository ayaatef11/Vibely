namespace SocialMedia.Core.Domain.DTOs.Requests.Block
{
    public class BlockDTO
    {
        public Guid BlockerId { set; get; }
        public Guid BlockedId { set; get; }
    }
}
