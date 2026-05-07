using SocialMedia.Core.Domain.DTOs.Requests.Like;

namespace SocialMedia.Application.Abstractions.PostAbstractions;
public interface IPostLikeService  
{
    ValueTask<string> LikeAsync(LikeRequest like);
    ValueTask<string> DisLikeAsync(DisLikeRequest like);
}
