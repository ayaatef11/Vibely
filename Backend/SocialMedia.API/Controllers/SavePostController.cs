using Microsoft.AspNetCore.Mvc;
namespace SocialMedia.API.Controllers;
[ApiController]
[Route("api/SavePost")]
public class SavePostController(ISavePostService _SavePostService) : ControllerBase
{

    [HttpPost("Save")]
    public async Task<IActionResult> Save(SavePostRequest savePost)
    { 
        var saveOperation = await _SavePostService.SaveAsync(savePost);
        return saveOperation == "Successfully" ?
            Ok(new Result
            {
                Message = "Post saved successfully"
            }) :
            BadRequest(saveOperation);
    }

    [HttpGet("Get/{userId}")]
    public async Task<IActionResult> Get(Guid userId)
    { 
        var posts = await _SavePostService.GetPosts(userId);
        return posts.Any() ?
            Ok(posts) :
            NotFound(new Result
            {
                Message = "No Saved Posts Found For This User"
            });
    }

    [HttpDelete("UnSave")]
    public async Task<IActionResult> UnSave(SavePostRequest savePost)
    { 
        var unSaveOperation = await _SavePostService.UnSaveAsync(savePost);
        return unSaveOperation == "Successfully" ?
            Ok(unSaveOperation) :
            BadRequest(unSaveOperation);
    }
}