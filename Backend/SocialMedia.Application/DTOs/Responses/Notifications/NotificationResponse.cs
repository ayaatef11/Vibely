namespace SocialMedia.Application.DTOs.Responses.Notifications;
public class NotificationResponse
{
    public Guid Id { get; set; }
    public Guid SenderId { get; set; }
    public string SenderName { get; set; } = string.Empty;
    public string SenderProfilePicture { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;   
    public string Message { get; set; } = string.Empty;
    public Guid? ReferenceId { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
}
