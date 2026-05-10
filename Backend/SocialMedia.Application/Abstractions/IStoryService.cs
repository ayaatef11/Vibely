using Microsoft.EntityFrameworkCore;

namespace SocialMedia.Application.Abstractions;
public interface IStoryService 
{
    ValueTask<IEnumerable<StoryResponse>> GetAllStories(Guid userId);
    ValueTask ViewStory(Guid userId, Guid storyId);
    ValueTask<IEnumerable<StoryView>> GetViewersForStory(Guid userId, Guid storyId);
    ValueTask ReactToStory(Guid userId, Guid storyId);
    ValueTask<CommentResponse> CommentToStory(AddCommentRequest comment);
    ValueTask<string> UploadAsync(UploadStoryRequest story);
    ValueTask<string> DeleteAsync(DeleteStoryRequest story);
    ValueTask<IEnumerable<StoryResponse>> GetUserStoriesAsync(Guid userId);
}
