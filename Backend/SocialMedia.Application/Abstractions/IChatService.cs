using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.DTOs.Requests.Chats;
using SocialMedia.Application.DTOs.Responses.Chats;
using SocialMedia.Core.Domain.Entities.Business.Chats;

namespace SocialMedia.Application.Abstractions;
public interface IChatService
{
    Task<ChatResponse> CreateChatAsync(Guid currentUserId, Guid otherUserId);

    Task<List<ChatResponse>> GetChatsAsync(Guid currentUserId);

    Task<List<MessageResponse>> GetMessagesAsync(Guid chatId, Guid currentUserId);

    Task<MessageResponse> SendMessageAsync(AddMessageRequest request);

    Task<MessageResponse> EditMessageAsync(Guid MessageId, EditMessageRequest request);
    Task DeleteMessageAsync(Guid messageId, Guid currentUserId);
}

