namespace SocialMedia.Application.DTOs.Requests.Notifications;
public class NotificationRequest
{
    public Guid RecipientId { get; set; }
    public Guid SenderId { get; set; }
    public NotificationType Type { get; set; }
    public string Message { get; set; }
    public Guid? ReferenceId { get; set; }
}
