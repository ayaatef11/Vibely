namespace SocialMedia.Application.Abstractions;
public interface ISavePostService  
{
    ValueTask<string> SaveAsync(SavePostRequest savePost);
    ValueTask<IEnumerable<PostResponse>> GetPosts(Guid userId);
    ValueTask<string> UnSaveAsync(SavePostRequest savePost);
}
