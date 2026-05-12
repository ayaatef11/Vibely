using AutoMapper;
using Microsoft.EntityFrameworkCore;
namespace SocialMedia.Application.Implementations;
public class UsersService(AppdbContext _context, IMapper _mapper) : IUsersService
{
    public async Task ReportUser(Guid userId, Guid reporterId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user is null) return;
        var report = new Report()
        {
            ReportedId = userId,
            ReporterId = reporterId
        };
        _context.Reports.Add(report);
        await _context.SaveChangesAsync();
    }


    public async Task<List<UserResponse>> SuggestUser(Guid userId)
    {
        var followingIds = await _context.Follows
    .Where(f => f.FollowerId == userId)
    .Select(f => f.FollowingId)
    .ToListAsync();

        var suggestions = await _context.Follows
            .Where(f => followingIds.Contains(f.FollowerId))
            .Select(f => f.Following)
            .Where(u => u.Id != userId && !followingIds.Contains(u.Id))
            .Distinct()
            .Take(10)
            .ToListAsync();
        return _mapper.Map<List<UserResponse>>(suggestions);
    }
    public List<UserResponse> SearchUser(string keyword)
    {
        var users = _context.Users.Where(u => u.FullName.Contains(keyword) || u.Location.Contains(keyword) || u.UserName.Contains(keyword) || u.Email.Contains(keyword)).ToList();
        return _mapper.Map<List<UserResponse>>(users);
    }
}
