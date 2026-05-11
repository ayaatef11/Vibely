using Microsoft.AspNetCore.Mvc;

namespace SocialMedia.API.Controllers;
[ApiController]
[Route("api/Follow")]
public class FollowController(IFollowerService _FollowerService) : ControllerBase
{
    [HttpPost("request")]
    public async Task<IActionResult> SendRequest(FollowRequest follow)
    {     
        var result = await _FollowerService.RequestFollowAsync(follow);
        return  Ok(result);
    }
    [HttpPost("unrequest")]
    public async Task<IActionResult> DeleteRequest(FollowRequest follow)
    {
        var result = await _FollowerService.UnrequestFollowAsync(follow);
        return  Ok(result);
    }

    [HttpPut("accept")]
    public async Task<IActionResult> Accept(FollowRequest follow)
    { 
        var result = await _FollowerService.AcceptFollowAsync(follow);
        return  Ok(result);
    }

    [HttpPut("reject")]
    public async Task<IActionResult> Reject(FollowRequest follow)
    {
        
        var result = await _FollowerService.RejectFollowAsync(follow);
        return  Ok(result);
    }

    [HttpDelete("unfollow")]
    public async Task<IActionResult> UnFollow(FollowRequest follow)
    {
        
        var result = await _FollowerService.UnFollowAsync(follow);
        return  Ok(result);
    }
    [HttpGet("find-people")]
    public async Task<IActionResult> FindPeople(Guid userId)
    {

        var result = await _FollowerService.FindPeople(userId);
        return Ok(result);
    }
    [HttpGet("view")]
    public async Task<IActionResult> ViewRequests(Guid profileId)
    {
        var result=await _FollowerService.ViewRequests(profileId);
        return Ok(result);
    }

    [HttpGet("Get/{profileId}")]
    public async Task<IActionResult> Get(Guid profileId)
    { 
        var result = await _FollowerService.GetFollowersAsync(profileId);
        return Ok(result) ;
    }
    [HttpGet("Get-following/{id}")]
    public async Task<IActionResult> GetFollowingWithStories(Guid id)
    { 
        var result = await _FollowerService.GetFollowingWithStoriesAsync(id);
        return  Ok(result);
    }
}