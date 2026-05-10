using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.DTOs.Responses.Posts;
using SocialMedia.Application.Helpers.Media;
using SocialMedia.Core.Domain.DTOs.Requests.Post; 

namespace SocialMedia.Application.Implementations;
public class PostService(AppdbContext _context, IMapper _mapper,IProfileService _profileService, IMainRepository<Post> _PostRepository) : IPostService
{
    public async ValueTask<PostResponse> AddPost(CreatePostRequest postRequest)
    {
        var _post = new Post()
        {
            Id = Guid.NewGuid(),
            ShareCount = 0,
            ReactsCount = 0,
            FeelingState = 0,
            CommentsCount = 0,
            Text = postRequest.Text,
            Title = postRequest.Title,
            CreatedAt = DateTime.UtcNow,
            ProfileId = postRequest.ProfileId,
            MediaUrls = (postRequest.Media != null && postRequest.Media.Any())
    ? System.Text.Json.JsonSerializer.Serialize(
        await Task.WhenAll(postRequest.Media.Select(m => PhotoHelper.Upload_photo(m)))
      ): null       
        };

        var addPostOperation = await _PostRepository.CreateAsync(_post);
        await _profileService.updatePostsCount(postRequest.ProfileId, true);
        return _mapper.Map<PostResponse>(_post);
    }

    public async  ValueTask<PostResponse> EditPost(UpdatePostRequest postRequest)
    {
        var _post = await _PostRepository.GetAsync(postRequest.Id);
        if (_post == null)
            throw new Exception("Post not found");

        _post.Text = postRequest.Text;
        _post.Title = postRequest.Title;
        _post.FeelingState = postRequest.FeelingState;
        await _PostRepository.UpdateAsync(_post, postRequest.Id);
        return _mapper.Map<PostResponse>(_post);
    }

    public async ValueTask DeletePost(Guid id)
    {
        var post = await _PostRepository.GetAsync(id);
        await _PostRepository.DeleteAsync(id);
        await _profileService.updatePostsCount(post.ProfileId, false);
    }
    public async ValueTask<List<PostResponse>?>SearchForPost(string keyword)
    {
        var posts=await _context.Posts.Where(x => x.Title.Contains( keyword) ||(x.Text!=null && x.Text.Contains(keyword))).ToListAsync();
        var result = _mapper.Map<List<PostResponse>?>(posts) ;
        return result;
    }

    public async ValueTask<IEnumerable<PostResponse>> GetTrendingPosts()//for public pages
    {
        var trendingPosts=await _context.Posts.Select(p => new 
        {
            Post=p,
            Score=(p.ReactsCount +p.CommentsCount +p.ShareCount) / EF.Functions.DateDiffHour(p.CreatedAt,DateTime.UtcNow)+1
        })
            .OrderByDescending(x => x.Score)
            .Take(10)
            .Select(x=>x.Post)
            .ToListAsync();
        return _mapper.Map<IEnumerable<PostResponse>>(trendingPosts);
    }
    public async ValueTask<long> GetSharesCount(Guid postId)
    {
        var sharesCount=await _context.Posts.Where(s => s.Id == postId).Select(s=>s.ShareCount).FirstOrDefaultAsync();
        return sharesCount;
    }
    public async ValueTask<long> GetLikesCount(Guid postId)
    {
        var likesCount = await _context.Posts.Where(s => s.Id == postId).Select(s => s.ReactsCount).FirstOrDefaultAsync();
        return likesCount;
    }
    public async ValueTask<IEnumerable<PostResponse>> GetUserPostsAsync(Guid profileId)
    {
        var user = await _context.Profiles
            .Include(x => x.Posts)
            .SingleOrDefaultAsync(x => x.Id == profileId);

        if (user == null)
            return new List<PostResponse>();

        var shareIds = await _context.Shares.Where(x => x.ProfileId == profileId).Select(x => x.PostId).ToListAsync();
        var postShared = await _context.Posts.Where(x => shareIds.Contains(x.Id)).ToListAsync();
        var finalPosts = user.Posts.ToList();
        finalPosts.AddRange(postShared);
        var result = _mapper.Map<List<PostResponse>>(finalPosts);
        return result;
    }
    public async ValueTask<PostResponse>GetPost(Guid postId)
    {
        var post=await _context.Posts.FirstOrDefaultAsync(x => x.Id == postId);
        return _mapper.Map<PostResponse>(post);
    }
    public async ValueTask<IEnumerable<PostResponseWithComments>> GetAllPosts(Guid userId)
    {
        var profileId=await _context.Users.Where(c => c.Id == userId).Select(c=>c.ProfileId).FirstOrDefaultAsync();
        var posts = await _context.Follows
        .Where(f => f.FollowerId == userId)
        .SelectMany(f => f.Following.Profile.Posts).Include(c=>c.Comments)
        .ToListAsync();
       
        var result = _mapper.Map<List<PostResponseWithComments>>(posts);
        foreach (var post in result)
        {
            post.IsLiked = await _context.PostLike
                .AnyAsync(l => l.PostId == post.Id && l.ProfileId == profileId);
        }

        return result;
 }
    public async ValueTask HidePost(Guid postId)
    {
        var post=await _context.Posts.FirstOrDefaultAsync(s => s.Id == postId);
        if (post == null) return;
        post.IsHidden = true;
        await _context.SaveChangesAsync();
    }

   
}