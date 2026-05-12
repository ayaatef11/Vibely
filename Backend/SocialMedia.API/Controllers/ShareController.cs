using Microsoft.AspNetCore.Mvc;

namespace SocialMedia.API.Controllers;
[ApiController]
[Route("api/Share")]
public class ShareController(ISharePostService _ShareService) : ControllerBase
{
    [HttpPost("Start")]
    public async Task<IActionResult> Start(StartShareRequest share)
    {
        
        var result = await _ShareService.Start(share);
        return Ok(result);
    }
    [HttpGet("share/{token}")]
    public async Task<IActionResult> OpenSharedPost(string token)
    {
        var share = await _ShareService.OpenSharedPost(token);
        if (share == null)
            return NotFound();

        return Ok(share);
    }

    [HttpDelete("revoke")]
    public async Task<IActionResult> Revoke(RevokeShareRequest share)
    { 
        await _ShareService.Revoke(share);
       return  Ok();
    }
}