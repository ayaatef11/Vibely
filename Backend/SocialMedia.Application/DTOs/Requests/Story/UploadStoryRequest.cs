using Microsoft.AspNetCore.Http;

namespace SocialMedia.Core.Domain.DTOs.Requests.Story;
public class UploadStoryRequest
{
    public string? Text { get; set; }
    public IFormFile? Image { set; get; }
    public IFormFile? Video { set; get; }
    public Guid ProfileId { set; get; }
}
