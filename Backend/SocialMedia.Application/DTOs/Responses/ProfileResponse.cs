namespace SocialMedia.Core.Domain.DTOs.Responses;
public class ProfileResponse
{
    public Guid Id {  get; set; }
    public int PostCount { set; get; }
    public int FollowerCount { set; get; }
    public int FollowingCount { set; get; }
    public string? Bio { get; set; } = string.Empty;
    public string FullName { set; get; } = string.Empty;
    public string UserName { set; get; } = string.Empty;
    public string? Website { set; get; } = string.Empty;
    public string? Location { set; get; } = string.Empty;
    public Guid UserId { set; get; }
    public bool IsFollowed {  set; get; }=false;
    public bool IsRequested { set; get; } = false;
    public List<PostResponse> Posts { get; set; }
}