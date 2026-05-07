namespace SocialMedia.Application.DTOs.Responses.Auth;
public class TokenResponse
{
    public bool Requires2FA { get; set; } = false;
    public string? Token { get; set; }
}
