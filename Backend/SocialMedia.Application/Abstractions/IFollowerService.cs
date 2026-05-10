using SocialMedia.Application.DTOs.Responses.Users;
using SocialMedia.Core.Domain.DTOs.Requests.Followers;

namespace SocialMedia.Application.Abstractions;
public interface IFollowerService
{
    ValueTask<string> AcceptFollowAsync(FollowRequest follow);

    ValueTask<ProfileResponse> UnFollowAsync(FollowRequest follow);
    ValueTask<ProfileResponse> RequestFollowAsync(FollowRequest follow);
    ValueTask<ProfileResponse> UnrequestFollowAsync(FollowRequest follow);
    ValueTask<ProfileResponse> RejectFollowAsync(FollowRequest followr);
    ValueTask<ICollection<UserResponse>> GetFollowersAsync(Guid profileId);
    ValueTask<ICollection<UserResponseWithStories>> GetFollowingWithStoriesAsync(Guid userId);
    Task<List<UserResponse>> ViewRequests(Guid profileId);
}

