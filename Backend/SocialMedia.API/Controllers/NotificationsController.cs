using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc; 
using SocialMedia.Core.Domain.Entities.Business.Profiles;

namespace SocialMedia.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class NotificationsController(INotificationsService _notificationService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Guid userId)
    {
        var result = await _notificationService.GetNotificationsAsync(userId);
        return Ok(result);
    }

    [HttpGet("unread")]
    public async Task<IActionResult> GetUnread([FromQuery] Guid userId)
    { 
        var result=await _notificationService.GetUnreadNotificationsAsync(userId);

        return Ok(result);
    }
    [HttpGet("unread-count")]
    public async Task<IActionResult> GetUnreadCount([FromQuery] Guid userId)
    {
        return Ok(await _notificationService.GetUnreadCountAsync(userId));
    }

    [HttpGet("by-type")]
    public async Task<IActionResult> GetByType([FromQuery] Guid userId, [FromQuery] NotificationType type)
    { 
        return Ok(await _notificationService.GetByTypeAsync(userId, type));
    }

    [HttpPut("{id}/read")]
    public async Task<IActionResult> MarkAsRead(Guid id)
    {
        await _notificationService.MarkAsReadAsync(id);
        return NoContent();
    }

    [HttpPut("read-all")]
    public async Task<IActionResult> MarkAllAsRead([FromQuery] Guid userId)
    {
        await _notificationService.MarkAllAsReadAsync(userId);
        return NoContent();
    }
}