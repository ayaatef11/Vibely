namespace SocialMedia.Core.Domain.DTOs.Requests.Story;
public class DeleteStoryDTO
{
    public Guid UserId { set; get; }
    public Guid StoryId { set; get; }
}
