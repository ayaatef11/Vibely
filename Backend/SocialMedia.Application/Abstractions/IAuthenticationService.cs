using SocialMedia.Application.DTOs.Requests.Authentication;
using SocialMedia.Application.DTOs.Responses.Auth;
using SocialMedia.Core.Domain.DTOs.Requests.Authentication;
using static SocialMedia.Application.Implementations.AuthenticationService;

namespace SocialMedia.Application.Abstractions;
public interface IAuthenticationService
{
    ValueTask<TokenResponse> LoginAsync(LoginDTO login,int? timeOutInMinutes);
    Task ChangePassword(Guid userId, ChangePasswordRequest request);
    ValueTask<TokenResponse> SignUpAsync(RegisterDTO register, int? timeOutInMinutes);
    ValueTask<string> RequestForgotPasswordAsync(string email);
    Task<SessionResponse> ChangeSessionTimeOut(Guid userId, int timeOut);
    Task<EnableTwoFAResponse> EnableTwoFA(Guid userId);
    Task VerifyTwoFA(Verify2FARequest request);

    ValueTask<TokenResponse> ResetPasswordAsync(ForgotPasswordDTO forgotPassword, int? timeOutInMinutes);
}
