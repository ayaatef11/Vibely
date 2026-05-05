using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Domain.DTOs.Requests.Comment;
using SocialMedia.Core.Domain.DTOs.Requests.Story;
using SocialMedia.Infrastructure.Domain.Entities.Business.Stories;

namespace SocialMedia.Application.Abstractions;
public interface IStoryService 
{
    ValueTask<IEnumerable<Story>> GetAllStories(Guid userId);
    ValueTask ViewStory(Guid userId, Guid storyId);
    ValueTask<IEnumerable<StoryView>> GetViewersForStory(Guid userId, Guid storyId);
    ValueTask ReactToStory(Guid userId, Guid storyId);
    ValueTask CommentToStory(AddCommentDTO comment);
    ValueTask<string> UploadAsync(UploadStoryDTO story);
    ValueTask<string> DeleteAsync(DeleteStoryDTO story);
    ValueTask<IEnumerable<Story>> GetUserStoriesAsync(Guid userId);
}
