using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace SocialMedia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController(AppdbContext _context) : ControllerBase
    {
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetMessages(Guid userId)
        {
            var currentUserId = User
                .FindFirst(ClaimTypes.NameIdentifier)?
                .Value;
            Guid.TryParse(currentUserId, out var parsedUserId);
            var messages = await _context.Messages
                .Where(x =>
                    (x.SenderId == parsedUserId
                     && x.ReceiverId == userId)
                    ||
                    (x.SenderId == userId
                     && x.ReceiverId == parsedUserId))
                .OrderBy(x => x.SentAt)
                .ToListAsync();

            return Ok(messages);
        }
    }
}
