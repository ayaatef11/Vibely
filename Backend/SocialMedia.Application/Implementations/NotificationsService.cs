using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.DTOs.Responses.Notifications;
using SocialMedia.Application.Hubs;
using SocialMedia.Core.Domain.Entities.Business.Profiles; 

namespace SocialMedia.Application.Implementations;
public class NotificationsService(AppdbContext _context, IMapper _mapper, IHubContext<NotificationsHub> _hubContext)
{
    public async Task SendNotificationAsync(
        Guid recipientId, Guid senderId, NotificationType type, string message, Guid? referenceId = null)
    {
        if (recipientId == senderId) return;

        var notification = new Notification
        {
            RecipientId = recipientId,
            SenderId = senderId,
            Type = type,
            Message = message,
            ReferenceId = referenceId,
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();

        var sender = await _context.Users.FindAsync(senderId);

        var response = new NotificationResponse
        {
            Id = notification.Id,
            SenderId = senderId,
            SenderName = sender?.FullName ?? string.Empty,
            SenderProfilePicture = sender?.ProfileImage ?? string.Empty,
            Type = type.ToString(),
            Message = message,
            ReferenceId = referenceId,
            IsRead = false,
            CreatedAt = notification.CreatedAt,
        };

        await _hubContext.Clients.Group(recipientId.ToString()).SendAsync("ReceiveNotification", response);
    }

    public async Task<List<NotificationResponse>> GetNotificationsAsync(Guid userId)
    {
        var notifications = await _context.Notifications
            .Include(n => n.Sender)
            .Where(n => n.RecipientId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();

        return MapList(notifications);
    }

    public async Task<List<NotificationResponse>> GetUnreadNotificationsAsync(Guid userId)
    {
        var notifications = await _context.Notifications
            .Include(n => n.Sender)
            .Where(n => n.RecipientId == userId && !n.IsRead)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();

        return MapList(notifications);
    }

    public async Task<List<NotificationResponse>> GetByTypeAsync(Guid userId, NotificationType type)
    {
        var notifications = await _context.Notifications
            .Include(n => n.Sender)
            .Where(n => n.RecipientId == userId && n.Type == type)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();

        return MapList(notifications);
    }

    public async Task<int> GetUnreadCountAsync(Guid userId)
        => await _context.Notifications
            .CountAsync(n => n.RecipientId == userId && !n.IsRead);

    public async Task MarkAsReadAsync(Guid notificationId)
    {
        var notification = await _context.Notifications.FindAsync(notificationId);
        if (notification is null) return;
        notification.IsRead = true;
        await _context.SaveChangesAsync();
    }

    public async Task MarkAllAsReadAsync(Guid userId)
    {
        var unread = await _context.Notifications
            .Where(n => n.RecipientId == userId && !n.IsRead)
            .ToListAsync();

        unread.ForEach(n => n.IsRead = true);
        await _context.SaveChangesAsync();
    }

    private List<NotificationResponse> MapList(List<Notification> list) =>
        list.Select(n => new NotificationResponse
        {
            Id = n.Id,
            SenderId = n.SenderId,
            SenderName = n.Sender?.FullName ?? string.Empty,
            SenderProfilePicture = n.Sender?.ProfileImage ?? string.Empty,
            Type = n.Type.ToString(),
            Message = n.Message,
            ReferenceId = n.ReferenceId,
            IsRead = n.IsRead,
            CreatedAt = n.CreatedAt,
        }).ToList();
}
