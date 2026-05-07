using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Context;
using SocialMedia.Core.Domain.DTOs.Requests.Comment;
using SocialMedia.Core.Domain.DTOs.Requests.Story;
using SocialMedia.Infrastructure.Domain.Entities.Business.Stories;

namespace SocialMedia.Application.Implementations;
public class StoryService(AppdbContext _context) : IStoryService
{
    public async ValueTask<IEnumerable<Story>> GetAllStories(Guid userId)
    {
        var user = await _context.Users.FindAsync(userId);
        var friendsIds = await _context.Follows.Select(x => x.FollowerId).ToListAsync();
        var stories =await _context.Stories.Where(s => friendsIds.Contains(s.Id)).ToListAsync() ;
        return stories;
    }
    public async ValueTask ViewStory(Guid userId, Guid storyId)
    {
        var story = await _context.Stories.FindAsync(userId);
        var viewStory = new StoryView()
        {
            UserId = userId,
            StoryId = storyId,
            ViewedAt = DateTime.UtcNow,
        };
        await _context.StoryViews.AddAsync(viewStory);
        await _context.SaveChangesAsync();

    }
    public async ValueTask<IEnumerable<StoryView>> GetViewersForStory(Guid userId, Guid storyId)
    {
        var storyViewers = await _context.StoryViews.Where(c => c.UserId == userId && c.StoryId == storyId).ToListAsync();
        return storyViewers;
    }
    public async ValueTask ReactToStory(Guid userId, Guid storyId)
    {
        var storyReaction = new StoryReaction()
        {
            UserId = userId,
            StoryId = storyId,
            ReactedAt = DateTime.UtcNow,
        };
        storyReaction.Count++;
        await _context.StoryReactions.AddAsync(storyReaction);
        await _context.SaveChangesAsync();
    }
   
    public async ValueTask CommentToStory(AddCommentRequest commentDto)
    {
        var comment = new Comment()
        {
            Text = commentDto.Text,
            ProfileId = commentDto.ProfileId,
            PostId = commentDto.PostId
        };
        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();
    }
    public async ValueTask<string> UploadAsync(UploadStoryRequest story)
    {
        var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == story.ProfileId);
        if (user == null)
            return "User Not Found Or Invalid User ID";

        var _story = new Story()
        {
            Id=Guid.NewGuid(),
            Text = story.Text,
            CreatedAt = DateTime.UtcNow,
            ProfileId = story.ProfileId,
        };

        if (story.Image != null)
        {
            using var imageMemoryStreem = new MemoryStream();
            await story.Image?.CopyToAsync(imageMemoryStreem);
            _story.ImageContentType = story.Image.ContentType;
            _story.Image = imageMemoryStreem.ToArray();
        }

        if (story.Video != null)
        {
            using var videoMemoryStreem = new MemoryStream();
            await story.Video?.CopyToAsync(videoMemoryStreem);
            _story.VideoContentType = story.Video?.ContentType;
            _story.Video = videoMemoryStreem.ToArray();
        }
        await _context.Stories.AddAsync(_story);
        var uploadOperation = await _context.SaveChangesAsync();
        return uploadOperation > 0 ? "Uploaded" :  "Failed To Upload Story";
    }

    public async ValueTask<string> DeleteAsync(DeleteStoryRequest story)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == story.UserId);
        if (user == null)
            return "User Not Found Or Invalid User ID";

        var _story = await _context.Stories.SingleOrDefaultAsync(x => x.Id == story.StoryId);
        if (_story == null)
            return "Story Not Found Or Invalid Story ID";

        _context.Stories.Remove(_story);
        var deleteOperation = await _context.SaveChangesAsync();
        return deleteOperation > 0 ?
            "Deleted" :
            "Failed To Delete Story";
    }

    public async ValueTask<IEnumerable<Story>> GetUserStoriesAsync(Guid profileId)
    {
        return await _context.Stories.Where(x => x.ProfileId == profileId).ToListAsync();
    }
}


