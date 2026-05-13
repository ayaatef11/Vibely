namespace SocialMedia.Application.Abstractions;
public interface IPostService  
{
    ValueTask<PostResponse> AddPost(CreatePostRequest  post);
    ValueTask<PostResponse> EditPost(UpdatePostRequest post);
    ValueTask DeletePost(Guid id);
    ValueTask<IEnumerable<PostResponse>> GetUserPostsAsync(Guid id);
    ValueTask<List<PostResponse>?> SearchForPost(string keyword);
    ValueTask<IEnumerable<PostResponseWithComments>> GetAllPosts(Guid profileId);
    ValueTask<PostResponse> GetPost(Guid postId,Guid ProfileId);
    ValueTask<IEnumerable<PostResponse>> GetTrendingPosts();
    ValueTask<long> GetSharesCount(Guid postId);
    ValueTask<long> GetLikesCount(Guid postId);
    ValueTask HidePost(Guid postId);
}

