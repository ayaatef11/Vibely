namespace SocialMedia.Infrastructure.Domain.Entities.Business.Profiles;
public class Notification : BaseEntity<Guid>
{
    public Guid RecipientId { get; set; }    
    public Guid SenderId { get; set; }          
    public NotificationType Type { get; set; }
    public string Message { get; set; } = string.Empty;
    public Guid? ReferenceId { get; set; }       
    public bool IsRead { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User Recipient { get; set; } = null!;
    public User Sender { get; set; } = null!;
}
