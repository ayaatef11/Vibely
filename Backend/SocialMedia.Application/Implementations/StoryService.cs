using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.Helpers;
namespace SocialMedia.Application.Implementations;
public class StoryService(AppdbContext _context,IMapper _mapper,PhotoHelper _photoHelper) : IStoryService
{
    public async ValueTask<IEnumerable<StoryResponse>> GetAllStories(Guid profileId)
    {
        var profile=await _context.Profiles.FirstOrDefaultAsync(p => p.Id == profileId);
        if (profile == null)
            throw new NotFoundException("Profile not found");

        var friendsIds = await _context.Follows.Select(x => x.FollowerId).ToListAsync();
        friendsIds.Add(profileId);
        var stories =await _context.Stories.Where(s => friendsIds.Contains(s.ProfileId)).ToListAsync();
        var result = _mapper.Map<IEnumerable<StoryResponse>>(stories);
        foreach(var story in result)
        {
            story.UserName = profile.UserName;
        }
        return result;
    }
    public async ValueTask<StoryResponse> ViewStory(Guid userId, Guid storyId)
    {
        var user =await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user is null)
            throw new NotFoundException("User not found");
        var story = await _context.Stories.FirstOrDefaultAsync(u=>u.Id==storyId);
        if (story is null)
            throw new NotFoundException("Story not found");

        var viewStory = new StoryView()
        {
            UserId = userId,
            StoryId = storyId,
            ViewedAt = DateTime.UtcNow,
        };
        await _context.StoryViews.AddAsync(viewStory);
        await _context.SaveChangesAsync();
        var result = _mapper.Map<StoryResponse>(story);
        result.UserName = user.UserName;
        return result;

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
            throw new NotFoundException( "Profile Not Found or Invalid Profile Id");

        var story = new Story()
        {
            Id=Guid.NewGuid(),
            Text = request.Text,
            CreatedAt = DateTime.UtcNow,
            ProfileId = request.ProfileId,
        };

        if (request.Image != null)
        {
            story.Image=await _photoHelper.UploadPhotoAsync(request.Image);
        }

        if (request.Video != null)
        {
            story.Video = await _photoHelper.UploadPhotoAsync(request.Video);

        }
        await _context.Stories.AddAsync(story);
        var uploadOperation = await _context.SaveChangesAsync();
         if(uploadOperation <=0 )throw new Exception( "Failed To Upload Story");
        var result = _mapper.Map<StoryResponse>(story);
        result.UserName = profile.UserName;
         return result;
    }

    public async ValueTask DeleteAsync(DeleteStoryRequest request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == request.UserId);
        if (user == null)
            throw new NotFoundException( "User Not Found Or Invalid User ID");

        var story = await _context.Stories.SingleOrDefaultAsync(x => x.Id == request.StoryId);
        if (story == null)
            throw new NotFoundException("Story Not Found Or Invalid Story ID");

        _context.Stories.Remove(story);
        var deleteOperation = await _context.SaveChangesAsync();
        if( deleteOperation <= 0 ) throw new BadRequestException("Failed To Delete Story");
    }

    public async ValueTask<IEnumerable<StoryResponse>> GetUserStoriesAsync(Guid profileId)
    {
        var profile=await _context.Profiles.FirstOrDefaultAsync(u=>u.Id == profileId);
        if (profile == null)
            throw new NotFoundException("Profile not found");
        var stories= await _context.Stories.Where(x => x.ProfileId == profileId).ToListAsync();
        var result = _mapper.Map<IEnumerable<StoryResponse>>(stories);
       
        foreach(var story in result)
        {
            story.UserName = profile.UserName;
        }
        return result;
    }
    
}


