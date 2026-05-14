namespace SocialMedia.Application.Abstractions;
public interface IFollowerService
{
    ValueTask AcceptFollowAsync(FollowRequest follow);

    ValueTask<ProfileResponse> UnFollowAsync(FollowRequest follow);
    ValueTask<ProfileResponse> RequestFollowAsync(FollowRequest follow);
    ValueTask<ProfileResponse> UnrequestFollowAsync(FollowRequest follow);
    ValueTask<ProfileResponse> RejectFollowAsync(FollowRequest followr);
    ValueTask<ICollection<UserResponse>> GetFollowersAsync(Guid profileId);
    ValueTask<ICollection<UserResponseWithStories>> GetFollowingWithStoriesAsync(Guid userId);
    ValueTask<ICollection<ProfileResponse>> FindPeople(Guid profileId);
    Task<List<ProfileResponse>> ViewRequests(Guid profileId);
}

