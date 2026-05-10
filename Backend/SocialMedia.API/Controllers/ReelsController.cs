using Microsoft.AspNetCore.Mvc;

namespace SocialMedia.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ReelsController(IReelsService _reelsService) : ControllerBase
{

    [HttpPost("Upload")]
    public async Task<IActionResult> Upload([FromForm] ReelsRequest real)
    {
        var result = await _reelsService.UploadAsync(real);
        return result == "Successfully" ? Ok(result) : BadRequest(result);
    }

    [HttpGet("Get{Id}")]
    public async Task<IActionResult> Get(Guid Id)
    {
        var result = await _reelsService.GetUserRealsAsync(Id);
        return result.Any() ? Ok(result) : NotFound("No Reals Found For This User");
    }

    [HttpDelete("Remove/{Id}")]
    public async Task<IActionResult> Remove(Guid Id)
    {
        var removeOperation = await _reelsService.RemoveAsync(Id);
        return removeOperation == "Successfully" ? Ok(removeOperation) : BadRequest(removeOperation);
    }
}