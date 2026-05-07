using Microsoft.AspNetCore.Mvc;
using SocialMedia.Core.Domain.DTOs.Requests.Profiles;

namespace SocialMedia.API.Controllers;
[ApiController]
[Route("api/Profile")]
public class ProfileController(IProfileService _ProfileService) : ControllerBase
{
    [HttpPut("edit")]
    public async Task<IActionResult> Edit([FromForm] EditProfileDTO edit)
    { 
        var result = await _ProfileService.EditAsync(edit);
        return Ok(result);
    }
    [HttpGet("followers")]
    public async Task<IActionResult>GetFollowers(Guid userId)
    {
        var result =await _ProfileService.GetFollowers(userId);
        return Ok(result);
    }
    [HttpGet("following")]
    public async Task<IActionResult>GetFollowing(Guid userId)
    {
        var result= await _ProfileService.GetFollowing(userId);
        return Ok(result);
    }
    [HttpGet("view")]
    public async Task<IActionResult>ViewProfile(Guid profileId)
    {
        var result=await _ProfileService.ViewProfile(profileId);
        return Ok(result);

    }
}
