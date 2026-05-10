using Microsoft.AspNetCore.Mvc;

namespace SocialMedia.API.Controllers;
[ApiController]
[Route("api/Like")]
public class LikeController(IPostLikeService _PostLikeService) : ControllerBase
{   
    [HttpPost("like")]
    public async Task<IActionResult> Like(LikeRequest like)
    {
        var likeOperation = await _PostLikeService.LikeAsync(like);

        return likeOperation == "Successfully" ? Ok("Like Added Succcessfully") : BadRequest(likeOperation);
    }

    [HttpDelete("dislike")]
    public async Task<IActionResult> Dislike(DisLikeRequest like)
    {
        var likeOperation = await _PostLikeService.DisLikeAsync(like);

        return likeOperation == "Successfully" ? Ok("Like Removed Succcessfully") : BadRequest("Invalid");
    }
}