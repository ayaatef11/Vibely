using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SocialMedia.Application.DTOs.Requests.Chats;
using SocialMedia.Application.DTOs.Responses.Chats;
using SocialMedia.Core.Domain.Entities.Business.Chats;
using System.Security.Claims;

namespace SocialMedia.Core.Hubs;
[Authorize]
public class ChatHub (IChatService _chatService) : Hub
{
   
    public async Task<MessageResponse> SendMessage(AddMessageRequest request)
    {

       var messageResponse= await _chatService.SendMessageAsync(request);

        await Clients.User(request.ReceiverId.ToString()).SendAsync("ReceiveMessage",messageResponse);
        // send back to sender too
        await Clients.Caller.SendAsync("ReceiveMessage",messageResponse);
        await Clients.User(request.SenderId.ToString()).SendAsync("MessageSent", new
            {
                request.ReceiverId,
                request.Content
            });
        return messageResponse;
    }
   
}
