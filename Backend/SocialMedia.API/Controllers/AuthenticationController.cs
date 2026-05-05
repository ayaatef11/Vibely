using Microsoft.AspNetCore.Mvc;
using SocialMedia.Core.Domain.DTOs.Requests.Authentication;
namespace SocialMedia.API.Controllers;
[ApiController]
[Route("api/Authentication")]
public class AuthenticationController(IAuthenticationService _authenticationService) : ControllerBase
{
    [HttpPost("signUp")]
    public async Task<IActionResult> SignUp(RegisterDTO register)
    {
        if (!ModelState.IsValid)
            return BadRequest(register);

        var siginUpOperation = await _authenticationService.SignUpAsync(register);
        return Ok(siginUpOperation);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDTO login)
    {
        if (!ModelState.IsValid)
            return BadRequest(login);

        var loginOperation = await _authenticationService.LoginAsync(login);
        if (loginOperation == "NotFound")
            return BadRequest("User Not Found");

        if (loginOperation == "InvalidPassword")
            return BadRequest("Invalid Password");

        return Ok(loginOperation);
    }

    [HttpPost("Forgot-Password-Request")]
    public async Task<IActionResult> RequestForgotPassword(string email)
    {
        if (string.IsNullOrEmpty(email))
            return BadRequest("Email is required");

        var result = await _authenticationService.RequestForgotPasswordAsync(email);
        if (result == "NotFound")
            return NotFound("User not found");

        return Ok(new Result(){ Message= result });
    }

    [HttpPut("Forgot-Password-Reset")]
    public async Task<IActionResult> ResetPassword(ForgotPasswordDTO forgotPassword)
    {
        if (!ModelState.IsValid)
            return BadRequest(forgotPassword);

        var resetOperation = await _authenticationService.ResetPasswordAsync(forgotPassword);
        if (resetOperation == "NotFound")
            return NotFound("User not found Or Invalid Email Address");

        if (resetOperation == "InvalidCode")
            return BadRequest("Invalid confirmation code");

        return Ok(resetOperation);
    }
}
