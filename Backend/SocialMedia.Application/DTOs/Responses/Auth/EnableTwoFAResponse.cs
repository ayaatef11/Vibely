namespace SocialMedia.Application.DTOs.Responses.Auth;
public class EnableTwoFAResponse
{
    public string Secret { get; set; }
    public string QrCodeUrl { get; set; }
}