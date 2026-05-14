using SocialMedia.Core.Domain.Entities.Business.Profiles;
namespace SocialMedia.Application.DTOs.Responses.Stories;
public class StoryResponse
{
    public Guid Id {  get; set; }
    public string? Text { get; set; }
    public string? UserName {  get; set; }
    public string? Image { set; get; }
    public string? Video { set; get; }
    public bool IsSeen { get; set; } = false;
    public Guid ProfileId { set; get; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
