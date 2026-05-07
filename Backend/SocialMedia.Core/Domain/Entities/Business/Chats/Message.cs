using SocialMedia.Infrastructure.Domain.Entities.Base;

namespace SocialMedia.Core.Domain.Entities.Business.Chats;
public sealed class Message : BaseEntity<Guid>
{

    public Guid ChatId { get; set; }

    public Chat Chat { get; set; } = default!;

    public string SenderId { get; set; } = default!;

    public string Content { get; set; } = default!;

    public bool IsEdited { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime SentAt { get; set; }

    public DateTime? EditedAt { get; set; }
}

