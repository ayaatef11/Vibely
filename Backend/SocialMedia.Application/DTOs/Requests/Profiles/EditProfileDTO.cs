using Microsoft.AspNetCore.Http;

namespace SocialMedia.Core.Domain.DTOs.Requests.Profiles;
public class EditProfileDTO
{
    public Guid UserId { get; set; }
    public string Bio { get; set; } = string.Empty;
    public string Website { set; get; } = string.Empty;
    public string FullName { set; get; } = string.Empty;
    public string UserName { set; get; } = string.Empty;
    public string Location { set; get; } = string.Empty;
    public IFormFile? ProfileImage { set; get; }
    public IFormFile? BackgroundImage { set; get; }
}
