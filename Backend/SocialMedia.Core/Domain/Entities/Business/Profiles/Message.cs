using SocialMedia.Infrastructure.Domain.Entities.Base;

namespace SocialMedia.Infrastructure.Domain.Entities.Business.Profiles;
public sealed class Message : BaseEntity<Guid>
{
    public Guid SenderId { get; set; }  
    public Guid ReceiverId { get; set; } 
    public string Content { get; set; }  
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
}
