namespace SocialMedia.Application.Abstractions;
public interface ICommentLikeService 
{
    ValueTask<CommentResponse> LikeAsync(LikeCommentRequest like);
    ValueTask<CommentResponse> DisLikeAsync(LikeCommentRequest like);
}
