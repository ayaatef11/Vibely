using Microsoft.AspNetCore.Mvc; 
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
    public async Task<IActionResult> Add(UploadStoryRequest story)//add signalr
    {       
        var uploadOperation = await _StoryService.UploadAsync(story);
        return uploadOperation == "Uploaded" ? Ok(new Result
             {
                 Message = "Story Added Successfully"
             }) : BadRequest(uploadOperation);
    }

    [HttpGet("view")]
    public async Task<IActionResult> ViewStory(Guid userId, Guid storyId)
    {
        await _StoryService.ViewStory(userId, storyId);
        return Ok();
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
    public async Task<IActionResult> CommentToStory(AddCommentRequest comment)
    {
        var result= await _StoryService.CommentToStory(comment);
        return Ok(result);
    }

    [HttpGet("get/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {  
        var result = await _StoryService.GetUserStoriesAsync(id);
        return Ok(result);
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete(DeleteStoryRequest story)
    { 
        var deleteOperation = await _StoryService.DeleteAsync(story);
        return deleteOperation == "Deleted" ?  Ok(deleteOperation) :  BadRequest(deleteOperation);
    }

}