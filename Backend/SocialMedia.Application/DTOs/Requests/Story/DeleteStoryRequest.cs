namespace SocialMedia.Core.Domain.DTOs.Requests.Story;
public class DeleteStoryRequest
{
    public Guid UserId { set; get; }
    public Guid StoryId { set; get; }
}
