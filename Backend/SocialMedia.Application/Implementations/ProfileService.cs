using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.Helpers;

namespace SocialMedia.Application.Implementations;
public class ProfileService(AppdbContext _context,IMapper _mapper) :  IProfileService
{
   public async Task<IEnumerable<ProfileResponse>> GetFollowers(Guid profileId)
    {
        var users=await _context.Follows.Include(u=>u.Follower).Where(u=>u.FollowingId== profileId)
            .Select(u => u.Follower).ToListAsync();

        var result = _mapper.Map<List<ProfileResponse>>(users);
        foreach(var user in result)
        {
            user.IsFollowed=await _context.Follows.AnyAsync(u=>u.FollowingId == user.Id && u.FollowerId==profileId && u.FollowState==FollowState.Accepted);
            user.IsRequested = await _context.Follows.AnyAsync(u => u.FollowingId == user.Id && u.FollowerId == profileId && u.FollowState == FollowState.Pending);

        }
        return result;
    }
    public async Task<IEnumerable<ProfileResponse>> GetFollowing(Guid profileId)
    {
        var users = await _context.Follows.Include(u => u.Following).Where(u => u.FollowerId == profileId)
            .Select(u => u.Following).ToListAsync();

        var result = _mapper.Map<List<ProfileResponse>>(users);
        return result;
    }
    public async Task<ProfileResponse> ViewProfile(Guid profileId)
    {
        var profile=await _context.Profiles.Include(u=>u.Posts).FirstOrDefaultAsync(u=>u.Id==profileId);
        if (profile == null)
            throw new NotFoundException("Profile is not found");
        var result =_mapper.Map<ProfileResponse>(profile); 
        return result;
    }
    public async ValueTask<ProfileResponse> EditAsync(EditProfileRequest request)
    {
        var profile = await _context.Profiles.FirstOrDefaultAsync(x => x.UserId == request.UserId);

        if (profile == null)
            throw new NotFoundException("Profile is not found");

        profile.Bio = request.Bio;
        profile.Website = request.Website;
        profile.Location = request.Location;
        profile.FullName = request.FullName;
        profile.UserName = request.UserName; 

        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.UserId);
        if (user is null)
            throw new NotFoundException("User is not found");
        user.Location = request.Location;
        user.FullName = request.FullName;
        user.UserName = request.UserName;

        await _context.SaveChangesAsync();
        return _mapper.Map<ProfileResponse>(profile);
    }
    public async Task updatePostsCount(Guid profileId,bool blus)
    {
        var profile = await _context.Profiles.FirstAsync(u => u.Id == profileId);
        if (profile == null) throw new NotFoundException("user is not found");
        if(blus)profile.PostCount++;
        else profile.PostCount--;
        await _context.SaveChangesAsync();
    }
}