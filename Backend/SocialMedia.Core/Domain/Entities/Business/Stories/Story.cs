using SocialMedia.Core.Domain.Entities.Business.Profiles;

namespace SocialMedia.Infrastructure.Domain.Entities.Business.Stories;
public class Story : BaseEntity<Guid>
{
    public string? Text { get; set; }
    public string? Image { set; get; }
    public string? Video { set; get; }
    public string? ImageContentType { get; set; }
    public string? VideoContentType { get; set; }

    public Guid ProfileId { set; get; }
    public UserProfile Profile { set; get; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

}
