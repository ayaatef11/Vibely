using SocialMedia.Application.DTOs.Responses.Users;
using SocialMedia.Core.Domain.DTOs.Requests.Followers;

namespace SocialMedia.Application.Abstractions;
public interface IFollowerService
{
    ValueTask<string> UnFollowAsync(FollowDTO follow);
    ValueTask<string> AcceptFollowAsync(FollowDTO follow);
    ValueTask<string> RequestFollowAsync(FollowDTO follow);
    ValueTask<string> UnrequestFollowAsync(FollowDTO follow);
    ValueTask<string> RejectFollowAsync(FollowDTO followr);
    ValueTask<ICollection<UserResponse>> GetFollowersAsync(Guid userId);
    ValueTask<ICollection<UserResponseWithStories>> GetFollowingWithStoriesAsync(Guid userId);
    Task<List<UserResponse>> ViewRequests(Guid userId);
}

