using Microsoft.AspNetCore.Mvc;
namespace SocialMedia.API.Controllers;
[ApiController]
[Route("api/SavePost")]
public class SavePostController(ISavePostService _SavePostService) : ControllerBase
{

    [HttpPost("Save")]
    public async Task<IActionResult> Save(SavePostRequest savePost)
    { 
        await _SavePostService.SaveAsync(savePost);
        return Ok();
    }

    [HttpGet("Get/{userId}")]
    public async Task<IActionResult> Get(Guid userId)
    { 
        var posts = await _SavePostService.GetPosts(userId);
        return Ok(posts);
    }

    [HttpDelete("UnSave")]
    public async Task<IActionResult> UnSave(SavePostRequest savePost)
    { 
        await _SavePostService.UnSaveAsync(savePost);
        return Ok();
    }
}