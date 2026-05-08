namespace SocialMedia.Core.Domain.Entities.Business.Chats;
public class ChatParticipant:BaseEntity<Guid>
{
    public Guid ChatId { get; set; }

    public Chat Chat { get; set; } = default!;
    public string Name {  get; set; }
    public Guid UserId { get; set; } = default!;
}