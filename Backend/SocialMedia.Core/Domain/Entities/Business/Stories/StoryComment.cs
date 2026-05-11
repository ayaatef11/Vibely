namespace SocialMedia.Core.Domain.Entities.Business.Stories;
public class StoryComment:BaseEntity<Guid>
{
    public string Text { set; get; }
    public long ReactCount { set; get; }
    public DateTime AddedAt { set; get; }

    public Story Story { set; get; }
    public Guid StoryId { set; get; }
    public Guid ProfileId { set; get; }
    public UserProfile Profile { set; get; }
}
