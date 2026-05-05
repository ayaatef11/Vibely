using Microsoft.AspNetCore.Mvc;
using SocialMedia.Core.Domain.DTOs.Requests.Followers;

namespace SocialMedia.API.Controllers;
[ApiController]
[Route("api/Follow")]
public class FollowController(IFollowerService _FollowerService) : ControllerBase
{
    [HttpPost("request")]
    public async Task<IActionResult> SendRequest(FollowDTO follow)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var requestOperation = await _FollowerService.RequestFollowAsync(follow);
        return requestOperation == "Successfully" ?
            Ok(new Result
            {
                Message = "Follow Sent Successfully"
            })
            : BadRequest(requestOperation);
    }
    [HttpPost("unrequest")]
    public async Task<IActionResult> DeleteRequest(FollowDTO follow)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var requestOperation = await _FollowerService.UnrequestFollowAsync(follow);
        return requestOperation == "Successfully" ?
            Ok(new Result
            {
                Message = "Follow Sent Successfully"
            })
            : BadRequest(requestOperation);
    }

    [HttpPut("accept")]
    public async Task<IActionResult> Accept(FollowDTO follow)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var acceptOperation = await _FollowerService.AcceptFollowAsync(follow);
        return acceptOperation == "Accepted" ?
           Ok(new Result
           {
               Message = "Follow Accepted Successfully"
           })
           : BadRequest(acceptOperation);
    }

    [HttpPut("reject")]
    public async Task<IActionResult> Reject(FollowDTO follow)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var rejectOperation = await _FollowerService.RejectFollowAsync(follow);
        return rejectOperation == "Rejected" ?
           Ok(new Result
           {
               Message = "Follow Rejected Successfully"
           })
           : BadRequest(rejectOperation);
    }

    [HttpDelete("unfollow")]
    public async Task<IActionResult> UnFollow(FollowDTO follow)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var unfollowOperation = await _FollowerService.UnFollowAsync(follow);
        return unfollowOperation == "Successfully" ?
           Ok(new Result
           {
               Message = "Follow Removed Successfully"
           })
           : BadRequest(unfollowOperation);
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