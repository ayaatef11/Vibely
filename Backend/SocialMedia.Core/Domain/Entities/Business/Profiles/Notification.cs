namespace SocialMedia.Infrastructure.Domain.Entities.Business.Profiles;
public class Notification : BaseEntity<Guid>
{
          
    public NotificationType Type { get; set; }
    public string Message { get; set; } = string.Empty;
    public Guid? ReferenceId { get; set; }       
    public bool IsRead { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid RecipientId { get; set; }
    public Guid SenderId { get; set; }
    public UserProfile Recipient { get; set; } = null!;
    public UserProfile Sender { get; set; } = null!;
}
