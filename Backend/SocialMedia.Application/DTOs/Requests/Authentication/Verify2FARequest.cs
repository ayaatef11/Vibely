namespace SocialMedia.Application.DTOs.Requests.Authentication;
public class Verify2FARequest
{
    public string UserId { get; set; }
    public string Code { get; set; }
}