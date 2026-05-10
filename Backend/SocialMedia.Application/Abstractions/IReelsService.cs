namespace SocialMedia.Application.Abstractions;
public interface IReelsService
{
    ValueTask<IEnumerable<ReelsResponse>> GetUserRealsAsync(Guid userId);
    ValueTask<string> RemoveAsync(Guid realId);
    ValueTask<string> UploadAsync(ReelsRequest request);
}
