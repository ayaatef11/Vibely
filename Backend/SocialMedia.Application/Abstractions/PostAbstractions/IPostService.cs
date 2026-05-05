using SocialMedia.Application.DTOs.Responses.Posts;
using SocialMedia.Core.Domain.DTOs.Requests.Post;

namespace SocialMedia.Application.Abstractions.PostAbstractions;
public interface IPostService  
{
    ValueTask<PostResponse> AddPost(CreatePostDTO  post);
    ValueTask<PostResponse> EditPost(UpdatePostDTO post);
    ValueTask DeletePost(Guid id);
    ValueTask<IEnumerable<PostResponse>> GetUserPostsAsync(Guid id);
    ValueTask<Post?> SearchForPost(string keyword);
    ValueTask<IEnumerable<PostResponseWithComments>> GetAllPosts(Guid userId);
    ValueTask<PostResponse> GetPost(Guid postId);
    ValueTask<IEnumerable<Post>> GetTrendingPosts();
    ValueTask<long> GetSharesCount(Guid postId);
    ValueTask<long> GetLikesCount(Guid postId);
    ValueTask HidePost(Guid postId);
}

