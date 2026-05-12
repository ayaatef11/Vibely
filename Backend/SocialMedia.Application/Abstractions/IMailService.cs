namespace SocialMedia.Application.Abstractions;
public interface IMailService
{
    ValueTask SendMailAsync(string email, string subject, string message);
}
