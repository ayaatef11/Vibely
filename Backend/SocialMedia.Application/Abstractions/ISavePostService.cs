namespace SocialMedia.Application.Abstractions;
public interface ISavePostService  
{
    ValueTask SaveAsync(SavePostRequest savePost);
    ValueTask<IEnumerable<PostResponse>> GetPosts(Guid userId);
    ValueTask UnSaveAsync(SavePostRequest savePost);
}
