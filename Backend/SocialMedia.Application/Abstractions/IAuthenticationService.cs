namespace SocialMedia.Application.Abstractions;
public interface IAuthenticationService
{
    ValueTask<TokenResponse> LoginAsync(LoginRequest login,int? timeOutInMinutes);
    Task ChangePassword(Guid userId, ChangePasswordRequest request);
    ValueTask<TokenResponse> SignUpAsync(RegisterRequest register, int? timeOutInMinutes);
    ValueTask RequestForgotPasswordAsync(string email);
    Task<SessionResponse> ChangeSessionTimeOut(Guid userId, int timeOut);
    Task<EnableTwoFAResponse> EnableTwoFA(Guid userId);
    Task VerifyTwoFASetUp(Verify2FARequest request);
    Task<TokenResponse> VerifyTwoFA(Verify2FARequest request);
    ValueTask<TokenResponse> ResetPasswordAsync(ForgotPasswordRequest forgotPassword, int? timeOutInMinutes);
}
