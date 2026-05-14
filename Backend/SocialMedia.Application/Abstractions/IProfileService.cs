namespace SocialMedia.Application.Abstractions;
public interface IProfileService 
{
    Task<IEnumerable<ProfileResponse>> GetFollowers(Guid profileId);
    Task<IEnumerable<ProfileResponse>> GetFollowing(Guid profileId);
    Task<ProfileResponse> ViewProfile(Guid profileId);
    ValueTask<ProfileResponse> EditAsync(EditProfileRequest profile);
    public Task updatePostsCount(Guid profileId, bool blus);

}
