using SocialMedia.Core.Domain.DTOs.Requests.Authentication;

namespace SocialMedia.Application.Abstractions;
public interface IAuthenticationService
{
    ValueTask<object> LoginAsync(LoginDTO login);
    ValueTask<object> SignUpAsync(RegisterDTO register);
    ValueTask<string> RequestForgotPasswordAsync(string email);
    ValueTask<object> ResetPasswordAsync(ForgotPasswordDTO forgotPassword);
}
