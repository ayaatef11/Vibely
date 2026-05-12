using Microsoft.AspNetCore.Identity;
using SocialMedia.Application.Abstractions;
using SocialMedia.Infrastructure.Domain.Entities.Security;

namespace SocialMedia.Application.Helpers.Messages;
public class ForgotPassword
{
    public async static ValueTask GenerateConfirmationCode(User user, IMailService mail, UserManager<User> userManager)
    {
        var code = Random.Shared.Next(100000, 999999).ToString();
        var generateResult = await userManager.SetAuthenticationTokenAsync(user, "ConfirmationCode", "ConfirmationCode", code);

        if (!generateResult.Succeeded)
            throw new Exception( "Invalid Generate Confirmation Code");

        var emailMessage = $@"
                <h1>Hello {user.FullName},</h1>
                <p>Thank you for registering with Sanpora.</p>
                <p>Your verification code is:</p>
                <h2>{code}</h2>
                <p>If you did not request this, please ignore this email.</p>
                <p>Thank you,<br>SocialMedia Team</p>
            ";

         await mail.SendMailAsync(user.Email, "Confirmation Code", emailMessage);
    }
}

