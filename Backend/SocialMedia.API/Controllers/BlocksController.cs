using Microsoft.AspNetCore.Mvc;
using SocialMedia.Core.Domain.DTOs.Requests.Block;

namespace SocialMedia.API.Controllers;
[ApiController]
[Route("api/Blocks")]
public class BlocksController(IBlockService _BlockService) : ControllerBase
{

    [HttpPost("block")]
    public async Task<IActionResult> Block(BlockDTO block)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var blockOperation = await _BlockService.BlockAsync(block);

        if (blockOperation == "UserFF")
            return BadRequest(new Result
            {
                Message = "User Is not found"
            });
        if (blockOperation == "UserAB")
            return BadRequest(new Result
            {
                Message = "User Alreay Blocked"
            });

        if (blockOperation == "UserAA")
            return BadRequest(new Result
            {
                Message = "User Can not block itself"
            });


        return blockOperation == "Successfully" ?
            Ok(new Result
            {
                Message = "User Blocked Successfully"
            })
            : BadRequest(blockOperation);
    }

    [HttpDelete("unblock")]
    public async Task<IActionResult> UnBlock(BlockDTO block)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var unBlockOperation = await _BlockService.UnBlockAsync(block);
        if (unBlockOperation == "UserNB")
            return Ok(new Result
            {
                Message = "User Not Blocked"
            });

        return unBlockOperation == "Successfully" ?
            Ok(new Result
            {
                Message = "User UnBlocked Successfully"
            })
            : BadRequest(unBlockOperation);
    }

    [HttpGet("GetBlockedUsers/{BlockerId}")]
    public async Task<IActionResult> GetBlockedUser(Guid BlockerId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var blockedUsers = await _BlockService.GetBlockedUserAsync(BlockerId);

        return blockedUsers.Any() ? Ok(blockedUsers) :
            NotFound(new Result
            {
                Message = "User Not Blocked Anyone"
            });
    }
}
