using SocialMedia.Application.DTOs.Responses.Chats;

namespace SocialMedia.Application.Abstractions;
public interface IChatService
{
    Task<Guid> CreateChatAsync(string currentUserId,string otherUserId);

    Task<List<ChatDto>> GetChatsAsync(string currentUserId);

    Task<List<MessageDto>> GetMessagesAsync(Guid chatId,string currentUserId);

    Task<MessageDto> SendMessageAsync(Guid chatId,string senderId,string content);

    Task<MessageDto> EditMessageAsync(Guid messageId,string currentUserId,string newContent);

    Task DeleteMessageAsync(Guid messageId,string currentUserId);
}

