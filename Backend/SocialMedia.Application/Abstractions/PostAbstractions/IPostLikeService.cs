using SocialMedia.Core.Domain.DTOs.Requests.Like;

namespace SocialMedia.Application.Abstractions.PostAbstractions;
public interface IPostLikeService  
{
    ValueTask<string> LikeAsync(LikeDTO like);
    ValueTask<string> DisLikeAsync(DisLikeDTO like);
}
