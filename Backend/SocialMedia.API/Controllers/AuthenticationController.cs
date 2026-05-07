using Microsoft.AspNetCore.Mvc;
using SocialMedia.Application.DTOs.Requests.Authentication;
using SocialMedia.Core.Domain.DTOs.Requests.Authentication;
namespace SocialMedia.API.Controllers;
[ApiController]
[Route("api/Authentication")]
public class AuthenticationController(IAuthenticationService _authenticationService) : ControllerBase
{
    [HttpPost("signUp")]
    public async Task<IActionResult> SignUp(RegisterRequest register)
    {
        var result = await _authenticationService.SignUpAsync(register,register.TimeOutInMinutes);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest login)
    { 
        var result = await _authenticationService.LoginAsync(login, login.TimeOutInMinutes);
        
        return Ok(result);
    }

    [HttpPost("Forgot-Password-Request")]
    public async Task<IActionResult> RequestForgotPassword(string email)
    { 
        var result = await _authenticationService.RequestForgotPasswordAsync(email);
        return Ok(result);
    }

    [HttpPut("Forgot-Password-Reset")]
    public async Task<IActionResult> ResetPassword(ForgotPasswordRequest forgotPassword)
    { 
        var result = await _authenticationService.ResetPasswordAsync(forgotPassword,forgotPassword.timeOutInMinutes);
    
        return Ok(result);
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(Guid userId, ChangePasswordRequest request)
    {
        await _authenticationService.ChangePassword(userId, request);
        return Ok();
    }
    [HttpGet("change-session-timeout")]
    public async Task<IActionResult> ChangeSessionTimeOut(Guid userId,int timeOut)
    {
        var result =await  _authenticationService.ChangeSessionTimeOut(userId, timeOut);
            return Ok(result);
    }
    [HttpPost("enable-2fa")]
    public async Task<IActionResult> EnableTwoFA(Guid userId)
    {
        var result =await _authenticationService.EnableTwoFA(userId);
        return Ok(result);
    }
    [HttpPost("verify-2fa")]
    public async Task<IActionResult> VerifyTwoFA(Verify2FARequest request)
    {
         await _authenticationService.VerifyTwoFA(request);
        return Ok();
    }

    //make it multilingual
    ///SIGNOUT
}
