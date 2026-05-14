using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace SocialMedia.Application.Implementations;
public class NotificationsService(AppdbContext _context, IMapper _mapper, IHubContext<NotificationsHub> _hubContext):INotificationsService
{
    public async Task SendNotificationAsync(NotificationRequest request)
    {
        if (request.RecipientId == request.SenderId) return;

        var notification = new Notification
        {
            RecipientId = request.RecipientId,
            SenderId = request.SenderId,
            Type = request.Type,
            Message = request.Message,
            ReferenceId = request.ReferenceId,
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();

        var sender = await _context.Users.FindAsync(request.SenderId);

        var response = new NotificationResponse
        {
            Id = notification.Id,
            SenderId = request.SenderId,
            SenderName = sender?.FullName ?? string.Empty,
            SenderProfilePicture = string.Empty,
            Type = request.Type.ToString(),
            Message = request.Message,
            ReferenceId = request.ReferenceId,
            IsRead = false,
            CreatedAt = notification.CreatedAt,
        };

        await _hubContext.Clients.Group(request.RecipientId.ToString()).SendAsync("ReceiveNotification", response);
    }

    public async Task<List<NotificationResponse>> GetNotificationsAsync(Guid profileId)
    {
        var notifications = await _context.Notifications
            .Include(n => n.Sender)
            .Where(n => n.RecipientId == profileId)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();

        return _mapper.Map<List<NotificationResponse>>(notifications);
    }

    public async Task<List<NotificationResponse>> GetUnreadNotificationsAsync(Guid profileId)
    {
        var notifications = await _context.Notifications
            .Include(n => n.Sender)
            .Where(n => n.RecipientId == profileId && !n.IsRead)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();

        return _mapper.Map<List<NotificationResponse>>(notifications);
    }

    public async Task<List<NotificationResponse>> GetByTypeAsync(Guid profileId, string type)
    {
        var notificationType = MapType(type);
        var notifications = await _context.Notifications
            .Include(n => n.Sender)
            .Where(n => n.RecipientId == profileId && n.Type == notificationType)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();

        return _mapper.Map<List<NotificationResponse>>(notifications);
    }

    private NotificationType MapType(string type)
    {  switch (type)
        {
            case "NewMessage":
                return NotificationType.NewMessage;
            case "NewPost":
                return NotificationType.NewPost;
            case "FriendRequest":
                return NotificationType.FriendRequest;
            case "FriendRequestAccepted":
                return NotificationType.FriendRequestAccepted;
            case "Like":
                return NotificationType.Like;
            case "Comment":
                return NotificationType.Comment;

        }
        throw new Exception("Error");
    }
    public async Task<int> GetUnreadCountAsync(Guid profileId)
        => await _context.Notifications.CountAsync(n => n.RecipientId == profileId && !n.IsRead);

    public async Task MarkAsReadAsync(Guid notificationId)
    {
        var notification = await _context.Notifications.FindAsync(notificationId);
        if (notification is null) return;
        notification.IsRead = true;
        await _context.SaveChangesAsync();
    }

    public async Task MarkAllAsReadAsync(Guid profileId)
    {
        var unread = await _context.Notifications
            .Where(n => n.RecipientId == profileId && !n.IsRead)
            .ToListAsync();

        unread.ForEach(n => n.IsRead = true);
        await _context.SaveChangesAsync();
    }

  
}
