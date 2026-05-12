namespace SocialMedia.Application.Abstractions;
public interface ISharePostService
{
    ValueTask Start(StartShareRequest start);
    ValueTask<string> Revoke(RevokeShareRequest revoke);
    Task<PostResponse?> OpenSharedPost(string token);
}
