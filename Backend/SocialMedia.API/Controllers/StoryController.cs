using Microsoft.AspNetCore.Mvc;
using SocialMedia.Application.DTOs.Requests.Stories;
namespace SocialMedia.API.Controllers;
[ApiController]
[Route("api/Story")]
public class StoryController(IStoryService _StoryService) : ControllerBase
{
    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllStories(Guid userId)
    {
        var result = await _StoryService.GetAllStories(userId);
        return Ok(result);
    }
    [HttpPost("add")]
    public async Task<IActionResult> Add(UploadStoryRequest request)
    {       
        var result = await _StoryService.UploadAsync(request);
        return Ok(result);
    }

    [HttpGet("view")]
    public async Task<IActionResult> ViewStory(Guid userId, Guid storyId)
    {
       var result= await _StoryService.ViewStory(userId, storyId);
        return Ok(result);
    }

    [HttpGet("get-viewers")]
    public async Task<IActionResult> GetViewersForStory(Guid userId, Guid storyId)
    {
        var viewers = await _StoryService.GetViewersForStory(userId, storyId);
        return Ok(viewers);
    }

    [HttpPost("add-react")]
    public async Task<IActionResult> ReactToStory(Guid userId, Guid storyId)
    {
        await _StoryService.ReactToStory(userId, storyId);
        return Ok();
    }

    [HttpPost("add-comment")]
    public async Task<IActionResult> CommentToStory(AddStoryCommentRequest request)
    {
        var result= await _StoryService.CommentToStory(request);
        return Ok(result);
    }

    [HttpGet("user/{profileId}")]
    public async Task<IActionResult> GetUserStoriesAsync(Guid profileId)
    {  
        var result = await _StoryService.GetUserStoriesAsync(profileId);
        return Ok(result);
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete(DeleteStoryRequest request)
    { 
       await _StoryService.DeleteAsync(request);
        return Ok();
    }

}