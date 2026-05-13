using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.Helpers;
namespace SocialMedia.Application.Implementations;
public class FollowerService(AppdbContext _context,IMapper _mapper,INotificationsService _notificationService) :  IFollowerService
{ 
    public async ValueTask<string> AcceptFollowAsync(FollowRequest request)
    {
        var sender = await _context.Profiles.SingleOrDefaultAsync(x => x.Id == request.Sender);
        if (sender == null)
            throw new NotFoundException( "Sender Not Found");

        var reciever = await _context.Profiles.SingleOrDefaultAsync(x => x.Id == request.Reciever);
        if (reciever == null)
            throw new NotFoundException("Reciever Not Found");

        var follow = await _context.Follows.SingleOrDefaultAsync
            (x => x.FollowerId == request.Sender && x.FollowingId == request.Reciever);

        if (follow == null)
            throw new BadRequestException("Follow Found");

        if (follow.FollowState == FollowState.Accepted)
            throw new BadRequestException("Follow Already Accepted");

        follow.FollowState = FollowState.Accepted;
        sender.FollowingCount++;
        reciever.FollowerCount++;
        var acceptOperation = await _context.SaveChangesAsync();
        var notificationRequest = new NotificationRequest()
        {
            RecipientId = sender.Id,
            SenderId = reciever.Id,
            Type = NotificationType.FriendRequestAccepted,
            Message = $"{reciever.FullName} accepted your friend request",
            ReferenceId = null
        };
        await _notificationService.SendNotificationAsync(notificationRequest);
        return acceptOperation > 0 ?"Accepted" : "Invalid";
    }

    public async ValueTask<ICollection<UserResponse>> GetFollowersAsync(Guid profileId)
    {
        var user = await _context.Profiles.Include(x => x.Followers).SingleOrDefaultAsync(x => x.Id == profileId);
        if (user == null || user.Followers.Count == 0) return new List<UserResponse>();
        var result = _mapper.Map<List<UserResponse>>(user.Followers.Select(x => x.Follower).ToList());
        return result;
    }
    public async ValueTask<ICollection<UserResponseWithStories>> GetFollowingWithStoriesAsync(Guid userid)
    { 
        var follows = await _context.Follows.Include(f => f.Following).Include(c=>c.Follower).ThenInclude(c=>c.Stories)
            .Where(f => f.FollowerId == userid).ToListAsync();

    if (!follows.Any())
        return new List<UserResponseWithStories>();

    var users = follows.Select(f => f.Following).ToList();

    var result = _mapper.Map<List<UserResponseWithStories>>(users);
        return result;
    }
    public async ValueTask<ICollection<ProfileResponse>>FindPeople(Guid profileId)
    {
        var followingIds = await _context.Follows
        .Where(f => f.FollowerId == profileId)
        .Select(f => f.FollowingId)
        .ToListAsync();
         
        var people = await _context.Profiles.Where(p => p.Id != profileId && !followingIds.Contains(p.Id))
            .ToListAsync();

        return _mapper.Map<List<ProfileResponse>>(people);
    }
    public async ValueTask<ProfileResponse> RejectFollowAsync(FollowRequest follow)
    {
        var sender = await _context.Profiles.SingleOrDefaultAsync(x => x.Id == follow.Sender);
        if (sender == null)
            throw new NotFoundException( "Sender Not Found");

        var receiver = await _context.Profiles.SingleOrDefaultAsync(x => x.Id == follow.Reciever);
        if (receiver == null)
            throw new NotFoundException("Reciever Not Found");

        var existedFollow = await _context.Follows.SingleOrDefaultAsync(x => x.FollowerId == follow.Sender && x.FollowingId == follow.Reciever);
        if (existedFollow == null)
            throw new NotFoundException("Follow Not Found");

        _context.Follows.Remove(existedFollow);
        var rejectOperation = await _context.SaveChangesAsync();

        if( rejectOperation <= 0 ) throw new BadRequestException("Invalid");
        return _mapper.Map<ProfileResponse>(receiver);
    }

    public async ValueTask<ProfileResponse> RequestFollowAsync(FollowRequest request)
    {
        var sender = await _context.Profiles.SingleOrDefaultAsync(x => x.Id == request.Sender);
        if (sender == null)
            throw new NotFoundException( "Sender Not Found");

        var receiver = await _context.Profiles.FirstOrDefaultAsync(x => x.Id == request.Reciever);
        if (receiver == null)
            throw new NotFoundException( "Reciever Not Found");

        var existedFollow = await _context.Follows
            .AnyAsync(x => x.FollowerId == request.Sender && x.FollowingId == request.Reciever);

        if (existedFollow) throw new BadRequestException( "User Already Following");

        var follow = new Follow()
        {
            FollowerId = request.Sender,
            FollowingId = request.Reciever,
            FollowState = FollowState.Pending
        };

        await _context.Follows.AddAsync(follow);
        var sendFollowOperation = await _context.SaveChangesAsync();

        if( sendFollowOperation <= 0) throw new BadRequestException("FollowRequestFailed");
        var notificationRequest = new NotificationRequest()
        {
            RecipientId = receiver.Id,
            SenderId = sender.Id,
            Type = NotificationType.FriendRequest,
            Message = $"{sender.FullName} sent you a friend request",
            ReferenceId = follow.Id
        };
        await _notificationService.SendNotificationAsync(notificationRequest);
        return _mapper.Map<ProfileResponse>(receiver);
    }


    public async ValueTask<ProfileResponse> UnFollowAsync(FollowRequest request)
    {
        var sender = await _context.Profiles.SingleOrDefaultAsync(x => x.Id == request.Sender);
        if (sender == null)
            throw new NotFoundException( "Sender Not Found");

        var receiver = await _context.Profiles.SingleOrDefaultAsync(x => x.Id == request.Reciever);
        if (receiver == null)
            throw new NotFoundException("Reciever Not Found");

        var existedFollow = await _context.Follows.SingleOrDefaultAsync(x => x.FollowerId == request.Sender && x.FollowingId == request.Reciever);
        if (existedFollow == null)
            throw new NotFoundException( "Follow Not Found");

        _context.Follows.Remove(existedFollow);
        sender.FollowingCount--;
        receiver.FollowerCount--;
        var unfollowOperation = await _context.SaveChangesAsync();

         if(unfollowOperation <= 0) throw new Exception( "Invalid");
        return _mapper.Map<ProfileResponse>(receiver);
    }

    public async  ValueTask<ProfileResponse> UnrequestFollowAsync(FollowRequest follow)
    {
        var sender = await _context.Profiles.SingleOrDefaultAsync(x => x.Id == follow.Sender);
        if (sender == null)
            throw new NotFoundException( "Sender Not Found");

        var receiver = await _context.Profiles.SingleOrDefaultAsync(x => x.Id == follow.Reciever);
        if (receiver == null)
            throw new NotFoundException( "Reciever Not Found");

        var existedFollow = await _context.Follows.Where(x => x.FollowerId == follow.Sender && x.FollowingId == follow.Reciever).FirstOrDefaultAsync();

        if (existedFollow == null)
            throw new NotFoundException( "Follow is not found ");

         _context.Follows.Remove(existedFollow);
        var sendFollowOperation = await _context.SaveChangesAsync();

         if(sendFollowOperation <=0) throw new BadRequestException( "FollowRequestFailed");
        return _mapper.Map<ProfileResponse>( receiver);
    }
    public async Task<List<ProfileResponse>>ViewRequests(Guid profileId)
    {
        var requests=await _context.Follows.Where(f=>f.FollowingId == profileId && f.FollowState==FollowState.Pending).Include(c=>c.Follower).Select(c=>c.Follower).ToListAsync();
        return _mapper.Map<List<ProfileResponse>>(requests);   
    }
}