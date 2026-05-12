using AutoMapper;
using Microsoft.AspNetCore.Mvc; 
namespace SocialMedia.API.Controllers;
[ApiController]
[Route("api/Comment")]
public class CommentController(ICommentService _CommentService, ICommentLikeService _CommentLikeService, IMapper _mapper) : ControllerBase
{
    [HttpPost("add")]
    public async Task<IActionResult> Add(AddCommentRequest comment)
    {
        var result = await _CommentService.AddComment(comment);
        return Ok(result);
    }

    [HttpPost("like")]
    public async Task<IActionResult> Like([FromBody] LikeCommentRequest likeComment)
    {
        var result = await _CommentLikeService.LikeAsync(likeComment);
        return Ok(result);
    }


    [HttpPut("edit")]
    public async Task<IActionResult> Edit(EditCommentRequest comment)
    { 
        var result = await _CommentService.EditComment(comment);

        return Ok(result);
    }

    [HttpDelete("dislike")]
    public async Task<IActionResult> Dislike(LikeCommentRequest dislikeComment)
    {
        var result = await _CommentLikeService.DisLikeAsync(dislikeComment);
        return Ok(result);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    { 
          await _CommentService.DeleteComment(id);

        return Ok();
    }
    [HttpGet("{postId}")]
    public async Task<IActionResult> GetComments(Guid postId)
    {
        if (postId == Guid.Empty)
            return BadRequest(new Result { Message = "Invalid Post ID" });

        var comments = await _CommentService.GetComments(postId);
        return comments != null ?
            Ok(comments) :
            NotFound(new Result
            {
                Message = "No Comments Found for this Post"
            });
    }
}
