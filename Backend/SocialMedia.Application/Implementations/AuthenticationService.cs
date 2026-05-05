using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SocialMedia.Core.Context;
using SocialMedia.Core.Domain.DTOs.Requests.Authentication;
using SocialMedia.Core.Domain.Entities.Business.Profiles;

namespace SocialMedia.Application.Implementations;
public class AuthenticationService(UserManager<User> _userManager, IConfiguration _configuration, AppdbContext _context,
        IMailService _mailService) : IAuthenticationService
{
    public async ValueTask<object> SignUpAsync(RegisterDTO register)
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

        return createProfileOperation > 0 ? GenerateTokenHelper.GenerateToken(SocialMediaUser, _configuration) :
            "Invalid Create Profile";
    }

    public async ValueTask<object> LoginAsync(LoginDTO login)
    {
        var user = await _userManager.FindByNameAsync(login.UserName);
        if (user == null)
            return "NotFound";

        var passwordCheck = await _userManager.CheckPasswordAsync(user, login.Password);

        if (!passwordCheck)
            return "Invalid password";

        return GenerateTokenHelper.GenerateToken(user, _configuration);
    }

    public async ValueTask<string> RequestForgotPasswordAsync(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        if (user == null)
            return "NotFound";

        return await ForgotPassword.GenerateConfirmationCode(user, _mailService, _userManager);
    }

    public async ValueTask<object> ResetPasswordAsync(ForgotPasswordDTO forgotPassword)
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
        return GenerateTokenHelper.GenerateToken(user, _configuration);
    }

   
}