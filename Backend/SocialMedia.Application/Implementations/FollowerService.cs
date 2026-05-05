using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.DTOs.Responses.Users;
using SocialMedia.Core.Domain.DTOs.Requests.Followers;
namespace SocialMedia.Application.Implementations;
public class FollowerService(AppdbContext _context,IMapper _mapper) :  IFollowerService
{ 
    public async ValueTask<string> AcceptFollowAsync(FollowDTO follow)
    {
        var _sender = await _context.Profiles.SingleOrDefaultAsync(x => x.SocialMediaUserId == follow.Sender);
        if (_sender == null)
            return "Sender Not Found";

        var _reciever = await _context.Profiles.SingleOrDefaultAsync(x => x.SocialMediaUserId == follow.Reciever);
        if (_reciever == null)
            return "Reciever Not Found";

        var _follow = await _context.Follows.SingleOrDefaultAsync
            (x => x.FollowerId == follow.Sender && x.FollowingId == follow.Reciever);

        if (_follow == null)
            return "Follow Found";

        if (_follow.FollowState == FollowState.Accepted)
            return "Follow Already Accepted";

        _follow.FollowState = FollowState.Accepted;
        _sender.FollowingCount++;
        _reciever.FollowerCount++;
        var acceptOperation = await _context.SaveChangesAsync();

        return acceptOperation > 0 ?"Accepted" : "Invalid";
    }

    public async ValueTask<ICollection<UserResponse>> GetFollowersAsync(Guid userid)
    {
        var user = await _context.Users.Include(x => x.Followers).SingleOrDefaultAsync(x => x.Id == userid);
        if (user == null || user.Followers.Count == 0) return new List<UserResponse>();
        var result = _mapper.Map<List<UserResponse>>(user.Followers.Select(x => x.Follower).ToList());
        return result;
    }
    public async ValueTask<ICollection<UserResponseWithStories>> GetFollowingWithStoriesAsync(Guid userid)
    { 
        var follows = await _context.Follows.Include(f => f.Following).ThenInclude(u => u.Profile).ThenInclude(p => p.Stories).Where(f => f.FollowerId == userid).ToListAsync();

    if (!follows.Any())
        return new List<UserResponseWithStories>();

    var users = follows.Select(f => f.Following).ToList();

    var result = _mapper.Map<List<UserResponseWithStories>>(users);
        return result;
    }
    public async ValueTask<string> RejectFollowAsync(FollowDTO follow)
    {
        var _sender = await _context.Users.SingleOrDefaultAsync(x => x.Id == follow.Sender);
        if (_sender == null)
            return "SenderNotFound";

        var _reciever = await _context.Users.SingleOrDefaultAsync(x => x.Id == follow.Reciever);
        if (_reciever == null)
            return "RecieverNotFound";

        var existedFollow = await _context.Follows.SingleOrDefaultAsync(x => x.FollowerId == follow.Sender && x.FollowingId == follow.Reciever);
        if (existedFollow == null)
            return "FollowNot Found";

        _context.Follows.Remove(existedFollow);
        var rejectOperation = await _context.SaveChangesAsync();

        return rejectOperation > 0 ?
            "Rejected" : "Invalid";
    }

    public async ValueTask<string> RequestFollowAsync(FollowDTO follow)
    {
        var _sender = await _context.Users.SingleOrDefaultAsync(x => x.Id == follow.Sender);
        if (_sender == null)
            return "Sender Not Found";

        var _receiver = await _context.Users.SingleOrDefaultAsync(x => x.Id == follow.Reciever);
        if (_receiver == null)
            return "RecieverNot Found";

        var existedFollow = await _context.Follows
            .AnyAsync(x => x.FollowerId == follow.Sender && x.FollowingId == follow.Reciever);

        if (existedFollow)
            return "User Already Following";

        var _follow = new Follow()
        {
            FollowerId = follow.Sender,
            FollowingId = follow.Reciever,
            FollowState = FollowState.Pending
        };

        await _context.Follows.AddAsync(_follow);
        var sendFollowOperation = await _context.SaveChangesAsync();

        return sendFollowOperation > 0 ? "Successfully" : "FollowRequestFailed";
    }


    public async ValueTask<string> UnFollowAsync(FollowDTO follow)
    {
        var _sender = await _context.Profiles.SingleOrDefaultAsync(x => x.SocialMediaUserId == follow.Sender);
        if (_sender == null)
            return "Sender Not Found";

        var _receiver = await _context.Profiles.SingleOrDefaultAsync(x => x.SocialMediaUserId == follow.Reciever);
        if (_receiver == null)
            return "RecieverN Found";

        var existedFollow = await _context.Follows.SingleOrDefaultAsync(x => x.FollowerId == follow.Sender && x.FollowingId == follow.Reciever);
        if (existedFollow == null)
            return "Follow Not Found";

        _context.Follows.Remove(existedFollow);
        _sender.FollowingCount--;
        _receiver.FollowerCount--;
        var unfollowOperation = await _context.SaveChangesAsync();

        return unfollowOperation > 0 ?
            "Successfully" : "Invalid";
    }

    public async  ValueTask<string> UnrequestFollowAsync(FollowDTO follow)
    {
        var _sender = await _context.Users.SingleOrDefaultAsync(x => x.Id == follow.Sender);
        if (_sender == null)
            return "Sender Not Found";

        var _receiver = await _context.Users.SingleOrDefaultAsync(x => x.Id == follow.Reciever);
        if (_receiver == null)
            return "RecieverNot Found";

        var existedFollow = await _context.Follows.Where(x => x.FollowerId == follow.Sender && x.FollowingId == follow.Reciever).FirstOrDefaultAsync();

        if (existedFollow == null)
            return "Follow is not found ";

      

         _context.Follows.Remove(existedFollow);
        var sendFollowOperation = await _context.SaveChangesAsync();

        return sendFollowOperation > 0 ? "Successfully" : "FollowRequestFailed";
    }
    public async Task<List<UserResponse>>ViewRequests(Guid userId)
    {
        var requests=await _context.Follows.Where(f=>f.FollowingId == userId && f.FollowState==FollowState.Pending).Include(c=>c.Follower).Select(c=>c.Follower).ToListAsync();
        return _mapper.Map<List<UserResponse>>(requests);   
    }
}