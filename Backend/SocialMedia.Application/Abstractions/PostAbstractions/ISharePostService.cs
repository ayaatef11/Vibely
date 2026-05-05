using SocialMedia.Application.DTOs.Responses.Posts;
using SocialMedia.Core.Domain.DTOs.Requests.Share;

namespace SocialMedia.Application.Abstractions.PostAbstractions;
public interface ISharePostService
{
    ValueTask<string> Start(StartShareDTO start);
    ValueTask<string> Revoke(RevokeShareDTO revoke);
    Task<PostResponse?> OpenSharedPost(string token);
}
