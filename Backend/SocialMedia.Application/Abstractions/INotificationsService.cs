using SocialMedia.Application.DTOs.Responses.Notifications;
using SocialMedia.Core.Domain.Entities.Business.Profiles; 

namespace SocialMedia.Application.Abstractions;
public interface INotificationsService
{
    Task SendNotificationAsync(Guid recipientId,Guid senderId,NotificationType type, string message,Guid? referenceId = null);

    Task<List<NotificationResponse>> GetNotificationsAsync(Guid userId);
    Task<List<NotificationResponse>> GetUnreadNotificationsAsync(Guid userId);
    Task<List<NotificationResponse>> GetByTypeAsync(Guid userId, NotificationType type);
    Task MarkAsReadAsync(Guid notificationId);
    Task MarkAllAsReadAsync(Guid userId);
    Task<int> GetUnreadCountAsync(Guid userId);
}
