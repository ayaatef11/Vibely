namespace SocialMedia.Application.Abstractions;
public interface ITokenBlocklistService
{
    void RevokeToken(string token, DateTime expiry);
    bool IsRevoked(string token);
}
