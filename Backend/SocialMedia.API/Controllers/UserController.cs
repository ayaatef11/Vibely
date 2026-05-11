using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc; 
namespace SocialMedia.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserController(IUsersService _usersService) : ControllerBase
{
    [HttpPost("report")]
    public async Task<IActionResult>ReportUser(Guid userId,Guid reporterId)
    {
        await _usersService.ReportUser(userId, reporterId);
        return Ok();
    }

    [HttpPost("suggest")]
    public async Task<IActionResult> SuggestUser(Guid userId)
    {
       var result=await _usersService.SuggestUser(userId);
        return Ok(result);
    }
    [HttpGet("search")]
    public IActionResult SearchUser(string keyword)
    {
        var result = _usersService.SearchUser(keyword);
        return Ok(result);
    }

  
}
