using SocialMedia.Application.DTOs.Responses.Users;
using SocialMedia.Core.Domain.DTOs.Requests.Followers;

namespace SocialMedia.Application.Abstractions;
public interface IFollowerService
{
    ValueTask<string> UnFollowAsync(FollowRequest follow);
    ValueTask<string> AcceptFollowAsync(FollowRequest follow);
    ValueTask<string> RequestFollowAsync(FollowRequest follow);
    ValueTask<string> UnrequestFollowAsync(FollowRequest follow);
    ValueTask<string> RejectFollowAsync(FollowRequest followr);
    ValueTask<ICollection<UserResponse>> GetFollowersAsync(Guid userId);
    ValueTask<ICollection<UserResponseWithStories>> GetFollowingWithStoriesAsync(Guid userId);
    Task<List<UserResponse>> ViewRequests(Guid userId);
}

