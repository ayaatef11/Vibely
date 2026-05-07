using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SocialMedia.Application.DTOs.Requests.Authentication;
using SocialMedia.Application.DTOs.Responses.Auth;
using SocialMedia.Core.Context;
using SocialMedia.Core.Domain.DTOs.Requests.Authentication;
using SocialMedia.Core.Domain.Entities.Business.Profiles; 

namespace SocialMedia.Application.Implementations;
public class AuthenticationService(UserManager<User> _userManager,IConfiguration _configuration, AppdbContext _context,
        IMailService _emailService) : IAuthenticationService
{
    public async ValueTask<object> SignUpAsync(RegisterDTO register,int? timeOutInMinutes)
    {
        var SocialMediaUser = new User()
        {
            Email = register.Email,
            UserName = register.UserName,
            FullName = register.FullName,
            Location = register.Location,
            PasswordHash = register.Password,
        };

        var addOperation = await _userManager.CreateAsync(SocialMediaUser, SocialMediaUser.PasswordHash);
        if (!addOperation.Succeeded)
            return addOperation.Errors.Select(e => e.Description).ToList();

        var _profile = new UserProfile()
        {
            PostCount = 0,
            FollowerCount = 0,
            FollowingCount = 0,
            UserName = SocialMediaUser.UserName,
            FullName = SocialMediaUser.FullName,
            SocialMediaUserId = SocialMediaUser.Id,
            Location = SocialMediaUser.Location,
        };
        await _context.Profiles.AddAsync(_profile);
        SocialMediaUser.ProfileId = _profile.Id;
        var createProfileOperation = await _context.SaveChangesAsync();

        return createProfileOperation > 0 ? GenerateTokenHelper.GenerateToken(SocialMediaUser, _configuration, timeOutInMinutes) :
            "Invalid Create Profile";
    }

    public async Task ChangePassword(Guid userId,ChangePasswordRequest request)
    {
        var user=await _context.Users.FirstOrDefaultAsync(u=>u.Id== userId);
        if (user==null) throw new Exception("User is not found");
        var result=await _userManager.ChangePasswordAsync(user, request.OldPassword,request.NewPassword);
        if (!result.Succeeded) { throw new Exception("Error has occurred"); }
    }
    public async Task<EnableTwoFAResponse> EnableTwoFA(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        await _userManager.ResetAuthenticatorKeyAsync(user);

        var key = await _userManager.GetAuthenticatorKeyAsync(user);

        var issuer = "Vibely";

        var otpauth = $"otpauth://totp/{issuer}:{user.Email}?secret={key}&issuer={issuer}&digits=6";

        
        return new EnableTwoFAResponse
        {
            Secret = key,
            QrCodeUrl = otpauth
        };
    }  

    public async Task VerifyTwoFA(Verify2FARequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);

        var isValid = await _userManager.VerifyTwoFactorTokenAsync(user, _userManager.Options.Tokens.AuthenticatorTokenProvider,request.Code);

        if (!isValid)
            throw new Exception("Invalid code");

        await _userManager.SetTwoFactorEnabledAsync(user, true);
    }
    public async ValueTask<object> LoginAsync(LoginDTO login,int? timeOutInMinutes)
    {
        var user = await _userManager.FindByNameAsync(login.UserName);
        if (user == null)
            return "NotFound";

        var passwordCheck = await _userManager.CheckPasswordAsync(user, login.Password);

        if (!passwordCheck)
            return "Invalid password";
        //        var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(
        //    code,
        //    false,
        //    false
        //);
        /*var result = await _signInManager.PasswordSignInAsync(
    model.Email,
    model.Password,
    false,
    lockoutOnFailure: false
);

if (result.RequiresTwoFactor)
{
    return "2FA_REQUIRED";
}*/
        await _emailService.SendMailAsync(user.Email, "New login to your account ", "There is a new login to your account");
        return GenerateTokenHelper.GenerateToken(user, _configuration, timeOutInMinutes);
    } 

    public async Task<SessionResponse> ChangeSessionTimeOut(Guid userId,int timeOut)
    {
        var user=await _context.Users.FirstOrDefaultAsync(u=>u.Id == userId);
        if (user is null) return null;
        string token =  GenerateTokenHelper.GenerateToken(user, _configuration, timeOut).Token;
        return new SessionResponse
        {
            Token = token,
            TimeOut = timeOut
        };
    }
   
    public async ValueTask<string> RequestForgotPasswordAsync(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        if (user == null)
            return "NotFound";

        return await ForgotPassword.GenerateConfirmationCode(user, _emailService, _userManager);
    }

    public async ValueTask<object> ResetPasswordAsync(ForgotPasswordDTO forgotPassword,int? timeOutInMinutes)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == forgotPassword.Email);
        if (user == null)
            return "NotFound";

        var token = await _userManager.GetAuthenticationTokenAsync(user, "ConfirmationCode", "ConfirmationCode");
        if (token != forgotPassword.Code)
            return "InvalidCode";

        var ResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        var resetResult = await _userManager.ResetPasswordAsync(user,
           ResetToken, forgotPassword.newPassword);

        if (!resetResult.Succeeded)
            return resetResult.Errors.Select(e => e.Description).ToList();

        await _userManager.RemoveAuthenticationTokenAsync(user, "ConfirmationCode", "ConfirmationCode");
        return GenerateTokenHelper.GenerateToken(user, _configuration, timeOutInMinutes);
    }

   
}