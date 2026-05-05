using Microsoft.AspNetCore.Mvc;
using SocialMedia.Core.Domain.DTOs.Requests.Post;
using SocialMedia.Infrastructure.Domain.Entities.Business.Posts;

namespace SocialMedia.API.Controllers;
[ApiController]
[Route("api/post")]
public partial class PostController(IPostService _PostService,IMainRepository<Post>_PostRepository,IProfileService _profileService) : ControllerBase
{
    [HttpPost("add")]
    public async Task<IActionResult> Add(CreatePostDTO post)
    {
       var result=await _PostService.AddPost(post);
        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Update(UpdatePostDTO post)
    {
        var result=await _PostService.EditPost(post);
        return  Ok(result);
    }

    [HttpGet("user/{profileId}")]
    public async Task<IActionResult> GetPostsForUser(Guid profileId)
    { 
        var result = await _PostService.GetUserPostsAsync(profileId);

        return result != null ? Ok(result) :NotFound(new Result{Message = "No Posts Found for this User"});
    }
    [HttpGet("{postId}")]
    public async Task<IActionResult> GetPost(Guid postId)
    {
        var result = await _PostService.GetPost(postId);

        return result != null ? Ok(result) : NotFound(new Result { Message = "No Posts Found for this User" });
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
       await  _PostService.DeletePost(id);
        return  Ok(new Result { Message = "Post Deleted Suceessfully"}) ;
    }
    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllPosts(Guid userId)
    {
        var posts =await _PostService.GetAllPosts(userId);
        return Ok(posts) ;
    }

    [HttpGet("show-posts")]
    public async Task<IActionResult> SearchForPost(string keyword)
    {
        var post = await _PostService.SearchForPost(keyword);
        return Ok(post);
    }

    [HttpGet("trending-posts")]
    public async Task<IActionResult> GetTrendingPosts()
    {
        var posts=await _PostService.GetTrendingPosts();
        return Ok(posts);
    }

    [HttpGet("shares-count")]
    public async Task<IActionResult>GetSharesCount(Guid postId)
    {
        var result=await _PostService.GetSharesCount(postId);
        return Ok(result);
    }

    [HttpGet("likes-count")]
    public async Task<IActionResult>GetLikesCount(Guid postId)
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