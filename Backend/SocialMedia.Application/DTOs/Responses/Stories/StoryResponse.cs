using SocialMedia.Core.Domain.Entities.Business.Profiles;
namespace SocialMedia.Application.DTOs.Responses.Stories;
public class StoryResponse
{
    public Guid Id {  get; set; }
    public string? Text { get; set; }
    public byte[]? Image { set; get; }
    public byte[]? Video { set; get; }
    public string? ImageContentType { get; set; }
    public string? VideoContentType { get; set; }

    public Guid ProfileId { set; get; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
