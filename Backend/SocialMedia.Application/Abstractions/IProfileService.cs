using SocialMedia.Application.DTOs.Responses.Users;
using SocialMedia.Core.Domain.DTOs.Requests.Profiles;

namespace SocialMedia.Application.Abstractions;
public interface IProfileService 
{
    Task<IEnumerable<UserResponse>> GetFollowers(Guid userId);
    Task<IEnumerable<UserResponse>> GetFollowing(Guid userId);
    Task<ProfileResponse> ViewProfile(Guid userId);
    ValueTask<ProfileResponse?> EditAsync(EditProfileDTO profile);
    public Task updatePostsCount(Guid userId, bool blus);

}
