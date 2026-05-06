using SocialMedia.Infrastructure.Domain.Enums;

namespace SocialMedia.Application.DTOs.Responses.Posts;
public class PostResponse
{
    public Guid Id { get; set; }

    public long ReactsCount { set; get; }
    public long ShareCount { set; get; }
    public long CommentsCount { set; get; }
    public DateTime CreatedAt { set; get; }
    public FeelingState? FeelingState { set; get; }
    public string Title { set; get; }  
    public string? Text { set; get; }  
    public string? MediaUrls { set; get; }
    public bool IsHidden { set; get; }  
    public string SaverIds { set; get; }
    public bool IsSaved { set; get; } 
    public bool IsReel { set; get; }
    public bool IsLiked { get; set; } = false;
    public Guid ProfileId { set; get; }

}
