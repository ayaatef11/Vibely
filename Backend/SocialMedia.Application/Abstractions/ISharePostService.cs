using SocialMedia.Application.DTOs.Responses.Posts;
using SocialMedia.Core.Domain.DTOs.Requests.Share;

namespace SocialMedia.Application.Abstractions;
public interface ISharePostService
{
    ValueTask<string> Start(StartShareRequest start);
    ValueTask<string> Revoke(RevokeShareRequest revoke);
    Task<PostResponse?> OpenSharedPost(string token);
}
