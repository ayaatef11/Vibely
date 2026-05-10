using SocialMedia.Infrastructure.Domain.Entities.Business.Stories;

namespace SocialMedia.Application.DTOs.Responses.Users;
public class UserResponseWithStories
{
    public string FullName { set; get; }
    public string Location { set; get; }
    public byte[]? ProfileImage { get; set; }
    public List<StoryResponse>? Stories { get; set; }
}

