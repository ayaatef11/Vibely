namespace SocialMedia.Application.DTOs.Responses.Chats;
public class ChatDto
{
    public Guid Id { get; set; }

    public string? LastMessage { get; set; }

    public DateTime? LastMessageDate { get; set; }
}
