namespace SocialMedia.Application.DTOs.Requests.Chats;
public class AddMessageRequest
{
    public Guid? ChatId { get; set; }
    public Guid SenderId { get; set; }
    public Guid ReceiverId { get; set; }
    public string Content { get; set; }
}
