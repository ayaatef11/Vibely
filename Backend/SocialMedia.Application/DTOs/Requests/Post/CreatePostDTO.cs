using Microsoft.AspNetCore.Http;
using SocialMedia.Infrastructure.Domain.Enums;

namespace SocialMedia.Core.Domain.DTOs.Requests.Post;
public class CreatePostDTO
{
    public FeelingState? FeelingState { set; get; }
    public string Title { set; get; } = string.Empty;
    public string? Text { set; get; } = string.Empty;
    public Guid ProfileId { set; get; }
    public List<IFormFile>?Media {  set; get; }
}