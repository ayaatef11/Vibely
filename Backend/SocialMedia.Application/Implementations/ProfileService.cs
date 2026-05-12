using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace SocialMedia.Application.Implementations;
public class ProfileService(AppdbContext _context,IMapper _mapper) :  IProfileService
{
   public async Task<IEnumerable<UserResponse>>? GetFollowers(Guid userId)
    {
        var users=await _context.Follows.Include(u=>u.Follower).Where(u=>u.FollowingId==userId)
            .Select(u => u.Follower).ToListAsync();
        if (users == null) return null;

        var result = _mapper.Map<List<UserResponse>>(users);
        return result;
    }
    public async Task<IEnumerable<UserResponse>> GetFollowing(Guid userId)
    {
        var users = await _context.Follows.Include(u => u.Following).Where(u => u.FollowerId == userId)
            .Select(u => u.Following).ToListAsync();
        if (users == null ||users.Count()==0) return null;

        var result = _mapper.Map<List<UserResponse>>(users);
        return result;
    }
    public async Task<ProfileResponse> ViewProfile(Guid profileId)
    {
        var profile=await _context.Profiles.Include(u=>u.Posts).FirstOrDefaultAsync(u=>u.Id==profileId);
        if (profile == null) return null;
        var result =_mapper.Map<ProfileResponse>(profile); 
        return result;
    }
    public async ValueTask<ProfileResponse?> EditAsync(EditProfileRequest request)
    {
        var profile = await _context.Profiles.
            FirstOrDefaultAsync(x => x.UserId == request.UserId);

        if (profile == null)
            return null ;

        profile.Bio = request.Bio;
        profile.Website = request.Website;
        profile.Location = request.Location;
        profile.FullName = request.FullName;
        profile.UserName = request.UserName;

        if (request.ProfileImage != null)
        {
            using var profileMemoryStream = new MemoryStream();
            await request.ProfileImage.CopyToAsync(profileMemoryStream);
            profile.ProfileImageContentType = request.ProfileImage.ContentType;
            profile.ProfileImage = profileMemoryStream.ToArray().ToString();
        }

        if (request.BackgroundImage != null)
        {
            using var backgroundMemoryStream = new MemoryStream();
            await request.BackgroundImage.CopyToAsync(backgroundMemoryStream);
            profile.BackgroundImageContentType = request.BackgroundImage.ContentType;
            profile.BackgroundImage = backgroundMemoryStream.ToArray().ToString();

        }
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == request.UserId);
        user.Location = request.Location;
        user.FullName = request.FullName;
        user.UserName = request.UserName;

        var editOperation = await _context.SaveChangesAsync();
        return _mapper.Map<ProfileResponse>(profile);
    }
    public async Task updatePostsCount(Guid profileId,bool blus)
    {
        var profile = await _context.Profiles.FirstAsync(u => u.Id == profileId);
        if (profile == null) throw new Exception("user is not found");
        if(blus)profile.PostCount++;
        else profile.PostCount--;
        await _context.SaveChangesAsync();
    }
}