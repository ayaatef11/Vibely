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

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(Guid userId,ChangePasswordRequest request)
    {
        await _authenticationService.ChangePassword(userId,request);
        return Ok();
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

    //make it multilingual

}
