using Microsoft.AspNetCore.Mvc;

namespace SocialMedia.API.Controllers;
[ApiController]
[Route("api/Like")]
public class LikeController(IPostLikeService _PostLikeService) : ControllerBase
{
    [HttpPost("like")]
    public async Task<IActionResult> Like(LikeRequest like)
    {
        await _PostLikeService.LikeAsync(like);
        return Ok();
    }

    [HttpDelete("dislike")]
    public async Task<IActionResult> Dislike(DisLikeRequest like)
    {
        await _PostLikeService.DisLikeAsync(like);
        return Ok();
    }
}