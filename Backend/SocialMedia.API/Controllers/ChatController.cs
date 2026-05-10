using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.DTOs.Requests.Chats;
using SocialMedia.Application.DTOs.Responses.Chats;
using System.Security.Claims;

namespace SocialMedia.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ChatController(IChatService _chatService) : ControllerBase
{

    [HttpGet]
    public async Task<IActionResult> GetChatsAsync(Guid currentUserId)
    {
        var result = await _chatService.GetChatsAsync(currentUserId);
        return Ok(result);
    }

    [HttpGet("messages")]
    public async Task<IActionResult> GetMessagesAsync(Guid chatId, Guid userId)
    {
        var result = await _chatService.GetMessagesAsync(chatId, userId);
        return Ok(result);
    }
    [HttpPost("{currentUserId}/{otherUserId}")]
    public async Task<IActionResult> CreateChatAsync([FromRoute] Guid currentUserId, [FromRoute] Guid otherUserId)
    {
        var result = await _chatService.CreateChatAsync(currentUserId, otherUserId);
        return Ok(result);
    }

    [HttpPost("send-message")]
    public async Task<IActionResult> SendMessageAsync(AddMessageRequest request)
    {
        var result = await _chatService.SendMessageAsync(request);
        return Ok(result);
    }

    [HttpPut("edit-message")]
    public async Task<IActionResult> EditMessageAsync(Guid messageId, EditMessageRequest request)
    {
        var result = await _chatService.EditMessageAsync(messageId, request);
        return Ok(result);
    }
    [HttpDelete("{messageId}")]
    public async Task<IActionResult> DeleteMessageAsync(Guid messageId, Guid currentUserId)
    {
       var result= await _chatService.DeleteMessageAsync(messageId, currentUserId);
        return Ok(result);
    }
}
