using AutoMapper;
using Microsoft.EntityFrameworkCore;
namespace SocialMedia.Application.Implementations;
public class StoryService(AppdbContext _context,IMapper _mapper) : IStoryService
{
    public async ValueTask<IEnumerable<StoryResponse>> GetAllStories(Guid profileId)
    {
        var friendsIds = await _context.Follows.Select(x => x.FollowerId).ToListAsync();
        friendsIds.Add(profileId);
        var stories =await _context.Stories.Where(s => friendsIds.Contains(s.ProfileId)).ToListAsync();
        return _mapper.Map<IEnumerable<StoryResponse>>(stories);
    }
    public async ValueTask<StoryResponse> ViewStory(Guid userId, Guid storyId)
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
        return _mapper.Map<StoryResponse>(story);

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
   
    public async ValueTask<StoryCommentResponse> CommentToStory(AddStoryCommentRequest request)
    {
        var comment = new StoryComment()
        {
            Text = request.Text,
            ProfileId = request.ProfileId,
            StoryId = request.StoryId
        };
        _context.StoryComments.Add(comment);
        await _context.SaveChangesAsync();
        return _mapper.Map<StoryCommentResponse>(comment);
    }

    public async ValueTask<StoryResponse> UploadAsync(UploadStoryRequest request)
    {
        var profile = await _context.Profiles.SingleOrDefaultAsync(x => x.Id == request.ProfileId);
        if (profile == null)
            throw new Exception( "Profile Not Found or Invalid Profile Id");

        var _story = new Story()
        {
            Id=Guid.NewGuid(),
            Text = request.Text,
            CreatedAt = DateTime.UtcNow,
            ProfileId = request.ProfileId,
        };

        if (request.Image != null)
        {
            using var imageMemoryStreem = new MemoryStream();
            await request.Image?.CopyToAsync(imageMemoryStreem);
            _story.ImageContentType = request.Image.ContentType;
            _story.Image = imageMemoryStreem.ToArray().ToString();
        }

        if (request.Video != null)
        {
            using var videoMemoryStreem = new MemoryStream();
            await request.Video?.CopyToAsync(videoMemoryStreem);
            _story.VideoContentType = request.Video?.ContentType;
            _story.Video = videoMemoryStreem.ToArray().ToString();
        }
        await _context.Stories.AddAsync(_story);
        var uploadOperation = await _context.SaveChangesAsync();
         if(uploadOperation <=0 )throw new Exception( "Failed To Upload Story");
         return _mapper.Map<StoryResponse>( _story );
    }

    public async ValueTask DeleteAsync(DeleteStoryRequest story)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == story.UserId);
        if (user == null)
            throw new Exception( "User Not Found Or Invalid User ID");

        var _story = await _context.Stories.SingleOrDefaultAsync(x => x.Id == story.StoryId);
        if (_story == null)
            throw new Exception("Story Not Found Or Invalid Story ID");

        _context.Stories.Remove(_story);
        var deleteOperation = await _context.SaveChangesAsync();
        if( deleteOperation <= 0 ) throw new Exception("Failed To Delete Story");
    }

    public async ValueTask<IEnumerable<StoryResponse>> GetUserStoriesAsync(Guid profileId)
    {
        var stories= await _context.Stories.Where(x => x.ProfileId == profileId).ToListAsync();
        return _mapper.Map<IEnumerable<StoryResponse>>(stories);
    }
    
}


