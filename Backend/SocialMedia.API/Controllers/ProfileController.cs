using Microsoft.AspNetCore.Mvc;
using SocialMedia.Core.Domain.DTOs.Requests.Profiles;

namespace SocialMedia.API.Controllers;
[ApiController]
[Route("api/Profile")]
public class ProfileController(IProfileService _ProfileService) : ControllerBase
{
    [HttpPut("edit")]
    public async Task<IActionResult> Edit([FromForm] EditProfileRequest request)
    { 
        var result = await _ProfileService.EditAsync(request);
        return Ok(result);
    }
    [HttpGet("followers")]
    public async Task<IActionResult>GetFollowers(Guid profileId)
    {
        var result =await _ProfileService.GetFollowers(profileId);
        return Ok(result);
    }
    [HttpGet("following")]
    public async Task<IActionResult>GetFollowing(Guid profileId)
    {
        var result= await _ProfileService.GetFollowing(profileId);
        return Ok(result);
    }
    [HttpGet("view")]
    public async Task<IActionResult>ViewProfile(Guid profileId)
    {
        var result=await _ProfileService.ViewProfile(profileId);
        return Ok(result);

    }
}
