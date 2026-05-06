namespace SocialMedia.Core.Domain.DTOs.Requests.Authentication;
public class LoginDTO
{
    public string UserName { set; get; }
    public string Password { set; get; }
    public int? timeOutInMinutes { set; get; }
}
