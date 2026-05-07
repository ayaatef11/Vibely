using Microsoft.AspNetCore.Mvc; 
namespace SocialMedia.API.Controllers;
[ApiController]
[Route("api/Story")]
public class StoryController(IStoryService _StoryService) : ControllerBase
{
    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllStories(Guid userId)
    {
        var stories = await _StoryService.GetAllStories(userId);
        return Ok(stories);
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
        await _StoryService.CommentToStory(comment);
        return Ok();
    }
    [HttpPost("add")]
    public async Task<IActionResult> Add(UploadStoryRequest story)//add signalr
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var uploadOperation = await _StoryService.UploadAsync(story);
        return uploadOperation == "Uploaded" ?
             Ok(new Result
             {
                 Message = "Story Added Successfully"
             }) :
             BadRequest(uploadOperation);
    }

    [HttpGet("get/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
      
        var stories = await _StoryService.GetUserStoriesAsync(id);
        return stories.Any() ?
            Ok(stories) :
            NotFound(new Result
            {
                Message = "No Stories Found For This User"
            });
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete(DeleteStoryRequest story)
    { 
        var deleteOperation = await _StoryService.DeleteAsync(story);
        return deleteOperation == "Deleted" ?
            Ok(deleteOperation) :
            BadRequest(deleteOperation);
    }

}