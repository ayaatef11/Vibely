using SocialMedia.Core.Domain.Entities.Business.Chats;

namespace SocialMedia.Application.DTOs.Responses.Chats;
public class ChatResponse
{
    public Guid Id { get; set; }

    public string? LastMessage { get; set; }
    public string Name { get; set; }

    public DateTime? LastMessageDate { get; set; }
    public ICollection<ChatParticipantResponse> Participants { get; set; } 
    public Guid ParticipantId {  get; set; }

}
