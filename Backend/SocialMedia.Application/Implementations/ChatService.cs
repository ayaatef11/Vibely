using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.DTOs.Responses.Chats;
using SocialMedia.Core.Domain.Entities.Business.Chats;

namespace SocialMedia.Application.Implementations;

public class ChatService(AppdbContext _context) : IChatService
{ 
    public async Task<Guid> CreateChatAsync(string currentUserId,string otherUserId)
    {
        // existing chat
        var existingChat =
            await _context.Chats
            .Include(x => x.Participants)
            .FirstOrDefaultAsync(x =>
                x.Participants.Any(p =>
                    p.UserId == currentUserId)  && x.Participants.Any(p =>
                    p.UserId == otherUserId) && !x.IsGroup);

        if (existingChat != null)
        {
            return existingChat.Id;
        }

        var chat = new Chat
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            IsGroup = false
        };

        chat.Participants.Add(new ChatParticipant{UserId = currentUserId});

        chat.Participants.Add(new ChatParticipant{UserId = otherUserId});

        _context.Chats.Add(chat);

        await _context.SaveChangesAsync();

        return chat.Id;
    }

    public async Task<List<ChatDto>> GetChatsAsync(string currentUserId)
    {
        var chats = await _context.Chats
            .Include(x => x.Messages)
            .Include(x => x.Participants)
            .Where(x =>x.Participants.Any(p =>p.UserId == currentUserId))
            .OrderByDescending(x =>x.Messages.Max(m =>(DateTime?)m.SentAt))
            .ToListAsync();

        return chats.Select(chat => new ChatDto
        {
            Id = chat.Id,

            LastMessage = chat.Messages
                .OrderByDescending(x => x.SentAt)
                .FirstOrDefault()?.Content,

            LastMessageDate = chat.Messages
                .OrderByDescending(x => x.SentAt)
                .FirstOrDefault()?.SentAt
        }).ToList();
    }

    public async Task<List<MessageDto>> GetMessagesAsync(Guid chatId, string currentUserId)
    {
        var isParticipant = await _context.ChatParticipants.AnyAsync(x =>x.ChatId == chatId&& x.UserId == currentUserId);

        if (!isParticipant)
        {
            throw new Exception("Unauthorized");
        }

        return await _context.Messages
            .Where(x => x.ChatId == chatId)
            .OrderBy(x => x.SentAt)
            .Select(x => new MessageDto
            {
                Id = x.Id,
                SenderId = x.SenderId,
                Content = x.Content,
                SentAt = x.SentAt,
                IsEdited = x.IsEdited
            })
            .ToListAsync();
    }

    public async Task<MessageDto> SendMessageAsync(Guid chatId,string senderId,string receiverId, string content)
    {
        //if (chatId is null) await CreateChatAsync(senderId, receiverId);
        var message = new Message
        {
            Id = Guid.NewGuid(),
            ChatId = chatId,
            SenderId = senderId,
            Content = content,
            SentAt = DateTime.UtcNow
        };

        _context.Messages.Add(message);

        await _context.SaveChangesAsync();

        return new MessageDto
        {
            Id = message.Id,
            SenderId = senderId,
            Content = content,
            SentAt = message.SentAt
        };
    }

    public async Task<MessageDto> EditMessageAsync(Guid messageId,string currentUserId,string newContent)
    {
        var message =await _context.Messages.FirstOrDefaultAsync(x => x.Id == messageId);

        if (message == null)
        {
            throw new Exception("Message not found");
        }

        if (message.SenderId != currentUserId)
        {
            throw new Exception("Unauthorized");
        }

        message.Content = newContent;

        message.IsEdited = true;

        message.EditedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return new MessageDto
        {
            Id = message.Id,
            SenderId = message.SenderId,
            Content = message.Content,
            SentAt = message.SentAt,
            IsEdited = true
        };
    }

    public async Task DeleteMessageAsync(Guid messageId, string currentUserId)
    {
        var message = await _context.Messages.FirstOrDefaultAsync(x => x.Id == messageId);

        if (message == null)
        {
            throw new Exception("Message not found");
        }

        if (message.SenderId != currentUserId)
        {
            throw new Exception("Unauthorized");
        }

        message.IsDeleted = true;

        message.Content = "Message deleted";

        await _context.SaveChangesAsync();
    }
}
