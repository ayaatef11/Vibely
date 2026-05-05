using SocialMedia.Application.DTOs.Responses;
using SocialMedia.Core.Domain.DTOs.Requests.Comment;

namespace SocialMedia.Application.Abstractions;
public interface ICommentLikeService 
{
    ValueTask<CommentResponse> LikeAsync(LikeCommentDTO like);
    ValueTask<CommentResponse> DisLikeAsync(LikeCommentDTO like);
}
