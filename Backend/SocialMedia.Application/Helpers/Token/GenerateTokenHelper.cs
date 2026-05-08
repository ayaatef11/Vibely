using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Application.DTOs.Responses.Auth;

namespace SocialMedia.Application.Helpers.Token;
public static class GenerateTokenHelper
{
    public static TokenResponse GenerateToken(User user, IConfiguration config,int? timeOutInMinutes)
    {
        var JwtOption = config.GetSection("JWT").Get<JWTOption>();

        var _claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim("ProfileId",user.ProfileId.ToString()),
                new Claim("UserName",user.UserName.ToString())
        };
        
        var _roles = user.Roles.ToList();

        foreach (var role in _roles)
        {
            _claims.Add(new Claim(ClaimTypes.Name, role.Name));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOption.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken
        (
            claims: _claims,
            issuer: JwtOption.Issuer,
            signingCredentials: creds,
            audience: JwtOption.Audience,
            expires: DateTime.UtcNow.AddMinutes(timeOutInMinutes??int.Parse(JwtOption.ExpireTime))
        );

        return new TokenResponse
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
        };
    }
}
