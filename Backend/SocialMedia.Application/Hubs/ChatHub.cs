using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace SocialMedia.Core.Hubs;
public class ChatHub (AppdbContext _context) : Hub
{
    public async Task SendMessage(Guid receiverId,string message)
    {
        var id = Context.User?
           .FindFirst(ClaimTypes.NameIdentifier)?
           .Value;
        Guid.TryParse(id, out var senderId);
        var newMessage = new Message
        {
            SenderId = senderId,
            ReceiverId = receiverId,
            Content = message
        };

        _context.Messages.Add(newMessage);

        await _context.SaveChangesAsync();

        await Clients.User(receiverId.ToString())
            .SendAsync("ReceiveMessage", new
            {
                id = newMessage.Id,
                senderId,
                receiverId,
                content = message,
                sentAt = newMessage.SentAt
            });
        // send back to sender too
        await Clients.Caller
            .SendAsync(
                "ReceiveMessage",
                new
                {
                    id = newMessage.Id,
                    senderId,
                    receiverId,
                    content = message,
                    sentAt = newMessage.SentAt
                });
        await Clients.User(senderId.ToString())
            .SendAsync("MessageSent", new
            {
                receiverId,
                message
            });
    }
}
