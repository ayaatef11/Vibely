namespace SocialMedia.Application.DTOs.Responses.Users;
public class UserResponse
{
    public Guid Id { get; set; }
    public string FullName { set; get; }
    public string Location { set; get; }
    public byte[]? ProfileImage { get; set; }
    public Guid? ProfileId { set; get; }

}
