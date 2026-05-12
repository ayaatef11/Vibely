using Microsoft.AspNetCore.Mvc;

namespace SocialMedia.API.Controllers;
[ApiController]
[Route("api/Blocks")]
public class BlocksController(IBlockService _BlockService) : ControllerBase
{

    [HttpPost("block")]
    public async Task<IActionResult> Block(BlockRequest block)
    {
        await _BlockService.BlockAsync(block);
        return Ok();
    }

    [HttpDelete("unblock")]
    public async Task<IActionResult> UnBlock(BlockRequest block)
    {
         await _BlockService.UnBlockAsync(block);
       return Ok();
    }

    [HttpGet("GetBlockedUsers/{BlockerId}")]
    public async Task<IActionResult> GetBlockedUser(Guid BlockerId)
    {
        var blockedUsers = await _BlockService.GetBlockedUserAsync(BlockerId);

        return blockedUsers.Any() ? Ok(blockedUsers) :
            NotFound(new Result
            {
                Message = "User Not Blocked Anyone"
            });
    }
}
