namespace SocialMedia.Core.Domain.DTOs.Requests.Stories;
public class DeleteStoryRequest
{
    public Guid UserId { set; get; }
    public Guid StoryId { set; get; }
}
