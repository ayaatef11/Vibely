namespace SocialMedia.Core.Domain.DTOs.Requests.Authentication;
public class LoginRequest
{
    public string UserName { set; get; }
    public string Password { set; get; }
    public int? TimeOutInMinutes { set; get; }
    public bool IsLoginNotificationsEnabled {  set; get; }= false
}
