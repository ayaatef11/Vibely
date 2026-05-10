using Microsoft.AspNetCore.Mvc;
using SocialMedia.Core.Domain.DTOs.Requests.Followers;

namespace SocialMedia.API.Controllers;
[ApiController]
[Route("api/Follow")]
public class FollowController(IFollowerService _FollowerService) : ControllerBase
{
    [HttpPost("request")]
    public async Task<IActionResult> SendRequest(FollowRequest follow)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _FollowerService.RequestFollowAsync(follow);
        return  Ok(result);
    }
    [HttpPost("unrequest")]
    public async Task<IActionResult> DeleteRequest(FollowRequest follow)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _FollowerService.UnrequestFollowAsync(follow);
        return  Ok(result);
    }

    [HttpPut("accept")]
    public async Task<IActionResult> Accept(FollowRequest follow)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _FollowerService.AcceptFollowAsync(follow);
        return  Ok(result);
    }

    [HttpPut("reject")]
    public async Task<IActionResult> Reject(FollowRequest follow)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _FollowerService.RejectFollowAsync(follow);
        return  Ok(result);
    }

    [HttpDelete("unfollow")]
    public async Task<IActionResult> UnFollow(FollowRequest follow)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _FollowerService.UnFollowAsync(follow);
        return  Ok(result);
    }
    [HttpGet("view")]
    public async Task<IActionResult> ViewRequests(Guid userId)
    {
        var result=await _FollowerService.ViewRequests(userId);
        return Ok(result);
    }

    [HttpGet("Get/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var followers = await _FollowerService.GetFollowersAsync(id);
        return followers.Any() ? Ok(followers)
            : NotFound(new Result
            {
                Message = "User Not Has Any Followes"
            });
    }
    [HttpGet("Get-following/{id}")]
    public async Task<IActionResult> GetFollowingWithStories(Guid id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var followers = await _FollowerService.GetFollowingWithStoriesAsync(id);
        return followers.Any() ? Ok(followers)
            : NotFound(new Result
            {
                Message = "User Not Has Any followings"
            });
    }
}