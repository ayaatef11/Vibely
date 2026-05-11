using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc; 

namespace SocialMedia.API.Controllers;

//[Authorize]
[ApiController]
[Route("api/[controller]")]
public class NotificationsController(INotificationsService _notificationService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Guid profileId)
    {
        var result = await _notificationService.GetNotificationsAsync(profileId);
        return Ok(result);
    }

    [HttpGet("unread")]
    public async Task<IActionResult> GetUnread([FromQuery] Guid profileId)
    { 
        var result=await _notificationService.GetUnreadNotificationsAsync(profileId);

        return Ok(result);
    }
    [HttpGet("unread-count")]
    public async Task<IActionResult> GetUnreadCount([FromQuery] Guid profileId)
    {
        var result = await _notificationService.GetUnreadCountAsync(profileId);
        return Ok(result);
    }

    [HttpGet("by-type")]
    public async Task<IActionResult> GetByType([FromQuery] Guid profileId, [FromQuery] string type)
    {
        var result = await _notificationService.GetByTypeAsync(profileId, type);
        return Ok(result);
    }

    [HttpPut("{id}/read")]
    public async Task<IActionResult> MarkAsRead(Guid id)
    {
        await _notificationService.MarkAsReadAsync(id);
        return NoContent();
    }

    [HttpPut("read-all")]
    public async Task<IActionResult> MarkAllAsRead([FromQuery] Guid profileId)
    {
        await _notificationService.MarkAllAsReadAsync(profileId);
        return NoContent();
    }
}