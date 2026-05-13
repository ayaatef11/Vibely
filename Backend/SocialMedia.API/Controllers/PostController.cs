using Microsoft.AspNetCore.Mvc;

namespace SocialMedia.API.Controllers;
[ApiController]
[Route("api/post")]
public partial class PostController(IPostService _PostService) : ControllerBase
{
    [HttpPost("add")]
    public async Task<IActionResult> Add(CreatePostRequest post)
    {
        var result = await _PostService.AddPost(post);
        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Update(UpdatePostRequest post)
    {
        var result = await _PostService.EditPost(post);
        return Ok(result);
    }

    [HttpGet("user/{profileId}")]
    public async Task<IActionResult> GetPostsForUser(Guid profileId)
    {
        var result = await _PostService.GetUserPostsAsync(profileId);

        return Ok(result);
    }
    [HttpGet("{postId}/{profileId}")]
    public async Task<IActionResult> GetPost(Guid postId, Guid profileId)
    {
        var result = await _PostService.GetPost(postId, profileId);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _PostService.DeletePost(id);
        return Ok(new Result { Message = "Post Deleted Suceessfully" });
    }
    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllPosts(Guid profileId)
    {
        var posts = await _PostService.GetAllPosts(profileId);
        return Ok(posts);
    }

    [HttpGet("search-posts")]
    public async Task<IActionResult> SearchForPost(string keyword)
    {
        var result = await _PostService.SearchForPost(keyword);
        return Ok(result);
    }

    [HttpGet("trending-posts")]
    public async Task<IActionResult> GetTrendingPosts()
    {
        var posts = await _PostService.GetTrendingPosts();
        return Ok(posts);
    }

    [HttpGet("shares-count")]
    public async Task<IActionResult> GetSharesCount(Guid postId)
    {
        var result = await _PostService.GetSharesCount(postId);
        return Ok(result);
    }

    [HttpGet("likes-count")]
    public async Task<IActionResult> GetLikesCount(Guid postId)
    {
        var result = await _PostService.GetLikesCount(postId);
        return Ok(result);
    }
    [HttpGet("hide-post")]
    public async Task<IActionResult> HidePost(Guid postId)
    {
        await _PostService.HidePost(postId);
        return Ok();
    }
}