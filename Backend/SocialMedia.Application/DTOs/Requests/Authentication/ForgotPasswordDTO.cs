namespace SocialMedia.Core.Domain.DTOs.Requests.Authentication;
public class ForgotPasswordDTO
{
    public string Email { set; get; }
    public string Code { set; get; }
    public string newPassword { set; get; }
    public int? timeOutInMinutes { set; get; }
}
