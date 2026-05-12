namespace SocialMedia.Application.Abstractions;
public interface IProfileService 
{
    Task<IEnumerable<UserResponse>> GetFollowers(Guid userId);
    Task<IEnumerable<UserResponse>> GetFollowing(Guid userId);
    Task<ProfileResponse> ViewProfile(Guid userId);
    ValueTask<ProfileResponse?> EditAsync(EditProfileRequest profile);
    public Task updatePostsCount(Guid userId, bool blus);

}
