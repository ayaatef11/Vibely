using SocialMedia.Application.DTOs.Responses.Posts;
using SocialMedia.Core.Domain.DTOs.Requests.SavePosts;

namespace SocialMedia.Application.Abstractions.PostAbstractions;
public interface ISavePostService  
{
    ValueTask<string> SaveAsync(SavePostRequest savePost);
    ValueTask<IEnumerable<PostResponse>> GetPosts(Guid userId);
    ValueTask<string> UnSaveAsync(SavePostRequest savePost);
}
