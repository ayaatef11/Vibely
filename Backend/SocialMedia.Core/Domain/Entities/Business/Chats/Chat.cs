namespace SocialMedia.Core.Domain.Entities.Business.Chats;
public class Chat:BaseEntity<Guid>
{
    public bool IsGroup { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<ChatParticipant> Participants { get; set; } = new List<ChatParticipant>();

    public ICollection<Message> Messages { get; set; } = new List<Message>();
}