using Microsoft.AspNetCore.Mvc;
using SocialMedia.Application.DTOs.Requests.Authentication;
using SocialMedia.Core.Domain.DTOs.Requests.Authentication;
namespace SocialMedia.API.Controllers;
[ApiController]
[Route("api/Authentication")]
public class AuthenticationController(IAuthenticationService _authenticationService) : ControllerBase
{
    [HttpPost("signUp")]
    public async Task<IActionResult> SignUp(RegisterDTO register, int timeOutInMinutes)
    {
        var result = await _authenticationService.SignUpAsync(register,timeOutInMinutes);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDTO login)
    { 
        var result = await _authenticationService.LoginAsync(login, login.timeOutInMinutes);
        if (result == "NotFound")
            return BadRequest("User Not Found");

        if (result == "InvalidPassword")
            return BadRequest("Invalid Password");

        return Ok(result);
    }

    [HttpPost("Forgot-Password-Request")]
    public async Task<IActionResult> RequestForgotPassword(string email)
    { 
        var result = await _authenticationService.RequestForgotPasswordAsync(email);
        if (result == "NotFound")
            return NotFound("User not found");

        return Ok(new Result(){ Message= result });
    }

    [HttpPut("Forgot-Password-Reset")]
    public async Task<IActionResult> ResetPassword(ForgotPasswordDTO forgotPassword)
    { 
        var result = await _authenticationService.ResetPasswordAsync(forgotPassword,forgotPassword.timeOutInMinutes);
        if (result == "NotFound")
            return NotFound("User not found Or Invalid Email Address");

        if (result == "InvalidCode")
            return BadRequest("Invalid confirmation code");

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

}
