using SocialMedia.Core.Domain.DTOs.Requests.Like;

namespace SocialMedia.Application.Abstractions;
public interface IPostLikeService  
{
    ValueTask LikeAsync(LikeRequest like);
    ValueTask DisLikeAsync(DisLikeRequest like);
}
