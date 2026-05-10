using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.DTOs.Responses.Users; 
namespace SocialMedia.Application.Implementations;
public class FollowerService(AppdbContext _context,IMapper _mapper) :  IFollowerService
{ 
    public async ValueTask<string> AcceptFollowAsync(FollowRequest follow)
    {
        var _sender = await _context.Profiles.SingleOrDefaultAsync(x => x.SocialMediaUserId == follow.Sender);
        if (_sender == null)
            throw new Exception( "Sender Not Found");

        var _reciever = await _context.Profiles.SingleOrDefaultAsync(x => x.SocialMediaUserId == follow.Reciever);
        if (_reciever == null)
            throw new Exception("Reciever Not Found");

        var _follow = await _context.Follows.SingleOrDefaultAsync
            (x => x.FollowerId == follow.Sender && x.FollowingId == follow.Reciever);

        if (_follow == null)
            throw new Exception("Follow Found");

        if (_follow.FollowState == FollowState.Accepted)
            throw new Exception("Follow Already Accepted");

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
    public async ValueTask<ProfileResponse> RejectFollowAsync(FollowRequest follow)
    {
        var _sender = await _context.Profiles.SingleOrDefaultAsync(x => x.SocialMediaUserId == follow.Sender);
        if (_sender == null)
            throw new Exception( "SenderNotFound");

        var receiver = await _context.Profiles.SingleOrDefaultAsync(x => x.SocialMediaUserId == follow.Reciever);
        if (receiver == null)
            throw new Exception("RecieverNotFound");

        var existedFollow = await _context.Follows.SingleOrDefaultAsync(x => x.FollowerId == follow.Sender && x.FollowingId == follow.Reciever);
        if (existedFollow == null)
            throw new Exception("FollowNot Found");

        _context.Follows.Remove(existedFollow);
        var rejectOperation = await _context.SaveChangesAsync();

        if( rejectOperation <= 0 ) throw new Exception("Invalid");
        return _mapper.Map<ProfileResponse>(receiver);
    }

    public async ValueTask<ProfileResponse> RequestFollowAsync(FollowRequest request)
    {
        var sender = await _context.Profiles.SingleOrDefaultAsync(x => x.SocialMediaUserId == request.Sender);
        if (sender == null)
            throw new Exception( "Sender Not Found");

        var receiver = await _context.Profiles.FirstOrDefaultAsync(x => x.SocialMediaUserId == request.Reciever);
        if (receiver == null)
            throw new Exception( "Reciever Not Found");

        var existedFollow = await _context.Follows
            .AnyAsync(x => x.FollowerId == request.Sender && x.FollowingId == request.Reciever);

        if (existedFollow) throw new Exception( "User Already Following");

        var follow = new Follow()
        {
            FollowerId = request.Sender,
            FollowingId = request.Reciever,
            FollowState = FollowState.Pending
        };

        await _context.Follows.AddAsync(follow);
        var sendFollowOperation = await _context.SaveChangesAsync();

        if( sendFollowOperation <= 0) throw new Exception("FollowRequestFailed");
       return _mapper.Map<ProfileResponse>(receiver);
    }


    public async ValueTask<ProfileResponse> UnFollowAsync(FollowRequest request)
    {
        var sender = await _context.Profiles.SingleOrDefaultAsync(x => x.SocialMediaUserId == request.Sender);
        if (sender == null)
            throw new Exception( "Sender Not Found");

        var receiver = await _context.Profiles.SingleOrDefaultAsync(x => x.SocialMediaUserId == request.Reciever);
        if (receiver == null)
            throw new Exception( "RecieverN Found");

        var existedFollow = await _context.Follows.SingleOrDefaultAsync(x => x.FollowerId == request.Sender && x.FollowingId == request.Reciever);
        if (existedFollow == null)
            throw new Exception( "Follow Not Found");

        _context.Follows.Remove(existedFollow);
        sender.FollowingCount--;
        receiver.FollowerCount--;
        var unfollowOperation = await _context.SaveChangesAsync();

         if(unfollowOperation <= 0) throw new Exception( "Invalid");
        return _mapper.Map<ProfileResponse>(receiver);
    }

    public async  ValueTask<ProfileResponse> UnrequestFollowAsync(FollowRequest follow)
    {
        var sender = await _context.Profiles.SingleOrDefaultAsync(x => x.SocialMediaUserId == follow.Sender);
        if (sender == null)
            throw new Exception( "Sender Not Found");

        var receiver = await _context.Profiles.SingleOrDefaultAsync(x => x.SocialMediaUserId == follow.Reciever);
        if (receiver == null)
            throw new Exception( "Reciever Not Found");

        var existedFollow = await _context.Follows.Where(x => x.FollowerId == follow.Sender && x.FollowingId == follow.Reciever).FirstOrDefaultAsync();

        if (existedFollow == null)
            throw new Exception( "Follow is not found ");

         _context.Follows.Remove(existedFollow);
        var sendFollowOperation = await _context.SaveChangesAsync();

         if(sendFollowOperation <=0) throw new Exception( "FollowRequestFailed");
        return _mapper.Map<ProfileResponse>( receiver);
    }
    public async Task<List<UserResponse>>ViewRequests(Guid userId)
    {
        var requests=await _context.Follows.Where(f=>f.FollowingId == userId && f.FollowState==FollowState.Pending).Include(c=>c.Follower).Select(c=>c.Follower).ToListAsync();
        return _mapper.Map<List<UserResponse>>(requests);   
    }
}