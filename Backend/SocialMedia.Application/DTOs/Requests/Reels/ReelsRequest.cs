using Microsoft.AspNetCore.Http;

namespace SocialMedia.Application.DTOs.Requests.Reels;
public class ReelsRequest
{
    public Guid UserId { set; get; }
    public IFormFile file { set; get; }
}
