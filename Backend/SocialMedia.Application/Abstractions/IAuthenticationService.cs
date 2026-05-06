using SocialMedia.Application.DTOs.Requests.Authentication;
using SocialMedia.Core.Domain.DTOs.Requests.Authentication;

namespace SocialMedia.Application.Abstractions;
public interface IAuthenticationService
{
    ValueTask<object> LoginAsync(LoginDTO login,int? timeOutInMinutes);
    Task ChangePassword(Guid userId, ChangePasswordRequest request);
    ValueTask<object> SignUpAsync(RegisterDTO register, int? timeOutInMinutes);
    ValueTask<string> RequestForgotPasswordAsync(string email);
    ValueTask<object> ResetPasswordAsync(ForgotPasswordDTO forgotPassword, int? timeOutInMinutes);
}
