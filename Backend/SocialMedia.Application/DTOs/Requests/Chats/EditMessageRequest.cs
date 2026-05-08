namespace SocialMedia.Application.DTOs.Requests.Chats;
public class EditMessageRequest
{
    public Guid CurrentUserId { get; set; }
    public string NewContent { get; set; }
}
