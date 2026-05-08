using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
namespace SocialMedia.Application.Hubs;


public class CustomUserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        var claims = connection.User?.Claims
       .Select(c => $"{c.Type} = {c.Value}");
        Console.WriteLine(string.Join("\n", claims ?? []));
        return connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? connection.User?.FindFirst("sub")?.Value; ;
    }
}