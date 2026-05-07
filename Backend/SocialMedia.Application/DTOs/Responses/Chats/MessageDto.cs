namespace SocialMedia.Application.DTOs.Responses.Chats;
public class MessageDto
{
    public Guid Id { get; set; }

    public string SenderId { get; set; } = default!;

    public string Content { get; set; } = default!;

    public bool IsEdited { get; set; }

    public DateTime SentAt { get; set; }
}
