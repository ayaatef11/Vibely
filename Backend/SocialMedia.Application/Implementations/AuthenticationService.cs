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
    public async ValueTask<TokenResponse> SignUpAsync(RegisterRequest register,int? timeOutInMinutes)
    {
        var User = new User()
        {
            Email = register.Email,
            UserName = register.UserName,
            FullName = register.FullName,
            Location = register.Location,
            PasswordHash = register.Password,
        };

        var addOperation = await _userManager.CreateAsync(User, User.PasswordHash);
        if (!addOperation.Succeeded)
            throw new Exception( addOperation.Errors.Select(e => e.Description).ToList().ToString());

        var _profile = new UserProfile()
        {
            PostCount = 0,
            FollowerCount = 0,
            FollowingCount = 0,
            UserName = User.UserName,
            FullName = User.FullName,
            UserId = User.Id,
            Location = User.Location,
        };
        await _context.Profiles.AddAsync(_profile);
        User.ProfileId = _profile.Id;
        await _context.SaveChangesAsync();

        return GenerateTokenHelper.GenerateToken(User, _configuration, timeOutInMinutes);
    }

    public async Task ChangePassword(Guid userId,ChangePasswordRequest request)
    {
        var user=await _context.Users.FirstOrDefaultAsync(u=>u.Id== userId);
        if (user==null) throw new Exception("User is not found");
        var result=await _userManager.ChangePasswordAsync(user, request.OldPassword,request.NewPassword);
        if (!result.Succeeded) { throw new Exception("Error has occurred"); }
    }
   
    public async ValueTask<TokenResponse> LoginAsync(LoginRequest request,int? timeOutInMinutes)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);
        if (user == null)
            throw new Exception( "NotFound");

        var passwordCheck = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!passwordCheck)
            throw new Exception( "Invalid password");
        if (user.TwoFactorEnabled)
        {
            return new TokenResponse
            {
                Requires2FA = true,
                Token =null
            };
        }
        if (request.IsLoginNotificationsEnabled)  await _emailService.SendMailAsync(user.Email, "New login to your account ", "There is a new login to your account");
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
   
    public async ValueTask RequestForgotPasswordAsync(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        if (user == null)
            throw new Exception("NotFound");
         await ForgotPassword.GenerateConfirmationCode(user, _emailService, _userManager);
    }

    public async ValueTask<TokenResponse> ResetPasswordAsync(ForgotPasswordRequest request, int? timeOutInMinutes)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == request.Email);
        if (user == null)
            throw new Exception( "NotFound");

        var token = await _userManager.GetAuthenticationTokenAsync(user, "ConfirmationCode", "ConfirmationCode");
        if (token != request.Code)
            throw new Exception( "InvalidCode");

        var ResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        var resetResult = await _userManager.ResetPasswordAsync(user,
           ResetToken, request.newPassword);

        if (!resetResult.Succeeded)
            throw new Exception( resetResult.Errors.Select(e => e.Description).ToList().ToString());

        await _userManager.RemoveAuthenticationTokenAsync(user, "ConfirmationCode", "ConfirmationCode");
        return GenerateTokenHelper.GenerateToken(user, _configuration, timeOutInMinutes);
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

    public async Task VerifyTwoFASetUp(Verify2FARequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);

        var isValid = await _userManager.VerifyTwoFactorTokenAsync(user, _userManager.Options.Tokens.AuthenticatorTokenProvider, request.Code);

        if (!isValid)
            throw new Exception("Invalid code");

        await _userManager.SetTwoFactorEnabledAsync(user, true);
    }
    public async Task<TokenResponse> VerifyTwoFA(Verify2FARequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);

        var isValid = await _userManager.VerifyTwoFactorTokenAsync(user, _userManager.Options.Tokens.AuthenticatorTokenProvider, request.Code);

        if (!isValid)
            throw new Exception("Invalid code");

        return GenerateTokenHelper.GenerateToken(user, _configuration,null);

    }
}