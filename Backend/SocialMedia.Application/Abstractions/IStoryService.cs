using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.DTOs.Requests.Stories;
using SocialMedia.Application.DTOs.Responses.Stories;

namespace SocialMedia.Application.Abstractions;
public interface IStoryService 
{
    ValueTask<IEnumerable<StoryResponse>> GetAllStories(Guid userId);
    ValueTask<StoryResponse> ViewStory(Guid userId, Guid storyId);
    ValueTask<IEnumerable<StoryView>> GetViewersForStory(Guid userId, Guid storyId);
    ValueTask ReactToStory(Guid userId, Guid storyId);
    ValueTask<StoryCommentResponse> CommentToStory(AddStoryCommentRequest comment);
    ValueTask<StoryResponse> UploadAsync(UploadStoryRequest story);
    ValueTask<string> DeleteAsync(DeleteStoryRequest story);
    ValueTask<IEnumerable<StoryResponse>> GetUserStoriesAsync(Guid ProfileId);
}
