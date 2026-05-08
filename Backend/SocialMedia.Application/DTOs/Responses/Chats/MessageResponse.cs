namespace SocialMedia.Application.DTOs.Responses.Chats;
public class MessageResponse
{
    public Guid Id { get; set; }

    public Guid SenderId { get; set; } = default!;

    public string Content { get; set; } = default!;

    public bool IsEdited { get; set; }

    public DateTime SentAt { get; set; }
}
