using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.DTOs.Requests.Notifications;
using SocialMedia.Application.Helpers;

namespace SocialMedia.Application.Implementations;

public class ChatService(AppdbContext _context,IMapper _mapper,INotificationsService _notificationService) : IChatService
{ 
    public async Task<ChatResponse> CreateChatAsync(Guid currentUserId,Guid otherUserId)
    {
        var existingChat = await _context.Chats.Include(x => x.Participants).FirstOrDefaultAsync(x => x.Participants.Any(p =>
                    p.UserId == currentUserId)  && x.Participants.Any(p => p.UserId == otherUserId) && !x.IsGroup);

        if (existingChat != null)
        {
            return _mapper.Map<ChatResponse>(existingChat);
        }
        var currentUser =await  _context.Users.FirstOrDefaultAsync(u=>u.Id== currentUserId);
        if (currentUser == null)
            throw new NotFoundException("Current user is not found");
        var otherUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == otherUserId);
        if (otherUser == null)
            throw new NotFoundException("Other user is not found");

        var chat = new Chat
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            IsGroup = false,
            AdminName=currentUser.FullName,
            Name=otherUser.FullName
        };

        chat.Participants.Add(new ChatParticipant{UserId = currentUserId,Name=currentUser.FullName});
        chat.Participants.Add(new ChatParticipant{UserId = otherUserId,Name=otherUser.FullName});
        _context.Chats.Add(chat);

        await _context.SaveChangesAsync();
        var result = _mapper.Map<ChatResponse>(chat);
        return result;
    }

    public async Task<List<ChatResponse>> GetChatsAsync(Guid currentUserId)
    {
        var user=await _context.Users.FirstOrDefaultAsync(u=>u.Id==currentUserId);
        var chats = await _context.Chats
            .Include(x => x.Messages)
            .Include(x => x.Participants)
            .Where(x => x.Participants.Any(p => p.UserId == currentUserId))
            .OrderByDescending(x =>x.Messages.Select(m => (DateTime?)m.SentAt).Max())
            .ToListAsync();

        return chats.Select(chat => {
            var LastMessage = (chat.Messages!=null && chat.Messages.Any()) ?chat.Messages.OrderByDescending(x => x.SentAt).First():null;
            return new ChatResponse
            {
                Id = chat.Id,
                LastMessage = LastMessage?.Content,
                Name = (user.FullName!=chat.AdminName) ?chat.AdminName : chat.Name,
                LastMessageDate = LastMessage?.SentAt,
                Participants=_mapper.Map<List<ChatParticipantResponse>>( chat.Participants),
                ParticipantId=chat.Participants.FirstOrDefault(u=>u.UserId !=currentUserId)?.UserId ?? currentUserId,
            };
       }).ToList();
    }
    public async Task<List<ChatResponse>>SearchChat(string chatName, Guid currentUserId) {
        var chats = await _context.Chats
                  .Include(x => x.Messages)
                  .Include(x => x.Participants)
                  .Where(x => x.Participants.Any(p => p.UserId == currentUserId) &&x.Name.Contains(chatName))
                  .OrderByDescending(x => x.Messages.Select(m => (DateTime?)m.SentAt).Max())
                  .ToListAsync();

        return chats.Select(chat => {
            var LastMessage = (chat.Messages != null && chat.Messages.Any()) ? chat.Messages.OrderByDescending(x => x.SentAt).First() : null;
            return new ChatResponse
            {
                Id = chat.Id,
                LastMessage = LastMessage?.Content,
                Name = chat.Name,
                LastMessageDate = LastMessage?.SentAt,
                Participants = _mapper.Map<List<ChatParticipantResponse>>(chat.Participants),
                ParticipantId = chat.Participants.FirstOrDefault(u => u.UserId != currentUserId)?.UserId ?? currentUserId,
            };
        }).ToList();
    }
    public async Task<List<MessageResponse>> GetMessagesAsync(Guid chatId, Guid currentUserId)
    {
        var isParticipant = await _context.ChatParticipants.AnyAsync(x =>x.ChatId == chatId&& x.UserId == currentUserId);

        if (!isParticipant)
        {
            throw new UnauthorizedException("Unauthorized");
        }

        var messages= await _context.Messages.Where(x => x.ChatId == chatId).OrderBy(x => x.SentAt).ToListAsync();
        return _mapper.Map<List<MessageResponse>>(messages);
    }

    public async Task<MessageResponse> SendMessageAsync(AddMessageRequest request)
    {
        ChatResponse chat=new ChatResponse() { };
        var receiver = await _context.Users.FirstOrDefaultAsync(c => c.Id == request.ReceiverId);
        if(receiver is null)
        {
            throw new NotFoundException("Receiver is not found");
        }
        var sender = await _context.Users.FirstOrDefaultAsync(c => c.Id == request.SenderId);
        if (sender is null) {
            throw new NotFoundException("Sender is not found");
        }
        if (request.ChatId is null) chat =await CreateChatAsync(request.SenderId, request.ReceiverId);
        var message =_mapper.Map<Message>(request);
        message.Id = Guid.NewGuid();
        message.SentAt = DateTime.UtcNow;
        _context.Messages.Add(message);

        await _context.SaveChangesAsync();
        var notificationRequest = new NotificationRequest()
        {
            RecipientId = receiver.ProfileId,
            SenderId = sender.ProfileId,
            Type = NotificationType.NewMessage,
            Message = $"{sender.FullName} sent you a message",
            ReferenceId = request.ChatId
        };
        await _notificationService.SendNotificationAsync(notificationRequest );
        return _mapper.Map<MessageResponse>(message);
    }

    public async Task<MessageResponse> EditMessageAsync(Guid MessageId,EditMessageRequest request)
    {
        var message =await _context.Messages.FirstOrDefaultAsync(x => x.Id == MessageId);

        if (message == null)
        {
            throw new NotFoundException("Message not found");
        }

        if (message.SenderId != request.CurrentUserId)
        {
            throw new UnauthorizedException("Unauthorized");
        }

        message.Content = request.NewContent;

        message.IsEdited = true;

        message.EditedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return new MessageResponse
        {
            Id = message.Id,
            SenderId = message.SenderId,
            Content = message.Content,
            SentAt = message.SentAt,
            IsEdited = true
        };
    }

    public async Task<MessageResponse> DeleteMessageAsync(Guid messageId, Guid currentUserId)
    {
        var message = await _context.Messages.FirstOrDefaultAsync(x => x.Id == messageId);

        if (message == null)
        {
            throw new NotFoundException("Message not found");
        }

        if (message.SenderId != currentUserId)
        {
            throw new UnauthorizedException("Unauthorized");
        }

        message.IsDeleted = true;

        message.Content = "Message deleted";

        await _context.SaveChangesAsync();
        return _mapper.Map<MessageResponse>(message);
    }
}
