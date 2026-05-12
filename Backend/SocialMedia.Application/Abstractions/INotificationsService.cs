namespace SocialMedia.Application.Abstractions;
public interface INotificationsService
{
    Task SendNotificationAsync(NotificationRequest request);

    Task<List<NotificationResponse>> GetNotificationsAsync(Guid userId);
    Task<List<NotificationResponse>> GetUnreadNotificationsAsync(Guid userId);
    Task<List<NotificationResponse>> GetByTypeAsync(Guid userId, string type);
    Task MarkAsReadAsync(Guid notificationId);
    Task MarkAllAsReadAsync(Guid userId);
    Task<int> GetUnreadCountAsync(Guid userId);
}
