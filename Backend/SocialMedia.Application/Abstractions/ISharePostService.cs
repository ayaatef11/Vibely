namespace SocialMedia.Application.Abstractions;
public interface ISharePostService
{
    ValueTask<string?> Start(StartShareRequest start);
    ValueTask Revoke(RevokeShareRequest revoke);
    Task<PostResponse?> OpenSharedPost(string token);
}
