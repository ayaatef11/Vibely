using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using static Org.BouncyCastle.Math.EC.ECCurve;
//using System.Net.Mail;

namespace SocialMedia.Application.Implementations;

public class MailService(IConfiguration _configuration) : IMailService
{
    public async ValueTask<string> SendMailAsync(string email, string subject, string toMessage)
    {
        try
        {
            var smtpOptions = _configuration.GetSection("SMTP").Get<SMTPOptions>();
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(smtpOptions.UserName,smtpOptions.Email));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = toMessage
            };

            message.Body = bodyBuilder.ToMessageBody();
            var smtpPort = smtpOptions.Port;

            using var client = new SmtpClient();

            SecureSocketOptions sslOptions = SecureSocketOptions.StartTls;
            if (!int.TryParse(smtpOptions.Port, out int portNumber))
                return "Invalid Port Number";

            await client.ConnectAsync(smtpOptions.Server, portNumber, sslOptions);


            if (!client.IsConnected)
            {
                throw new InvalidOperationException("Failed to connect to the SMTP server.");
            }

            await client.AuthenticateAsync(smtpOptions.UserName, smtpOptions.Password);

            if (!client.IsAuthenticated)
            {
                return "Not authenticated";
            }
            var response = await client.SendAsync(message);
            await client.DisconnectAsync(true);
            return "Successfully";

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Email Error: {ex.Message}");
            if (ex.InnerException != null)
                Console.WriteLine($"Inner error: {ex.InnerException.Message}");

            throw new Exception("Failed to send email", ex);
        }
    }
}