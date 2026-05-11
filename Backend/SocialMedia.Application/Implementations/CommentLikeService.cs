using AutoMapper;
using Microsoft.EntityFrameworkCore; 

namespace SocialMedia.Application.Implementations;
public class CommentLikeService(AppdbContext _context,IMapper _mapper,INotificationsService _notificationService) :  ICommentLikeService
{
    public async ValueTask<CommentResponse> DisLikeAsync(LikeCommentRequest request)
    {
        var like = await _context.CommentLikes.SingleOrDefaultAsync(x => x.CommentId == request.CommentId);
        if (like == null)
            throw new Exception("Like Not Found Or Invalid Like ID");

        var post = await _context.Posts.SingleOrDefaultAsync(x => x.Id == request.PostId);
        if (post == null)
            throw new Exception("Post Not Found Or Invalid Post ID");

        var profile = await _context.Profiles.SingleOrDefaultAsync(x => x.Id == request.ProfileId);
        if (profile == null)
            throw new Exception("User Not Found Or Invalid User ID");

        var comment = await _context.Comments.SingleOrDefaultAsync(x => x.Id == request.CommentId);
        if (comment == null)
            throw new Exception("Comment Not Found Or Invalid Comment ID");

        comment.ReactCount--;
        _context.CommentLikes.Remove(like);
        var dislikeOperation = await _context.SaveChangesAsync();
        return _mapper.Map<CommentResponse>(comment);
    }

    public async ValueTask<CommentResponse> LikeAsync(LikeCommentRequest request)
    {
        var post = await _context.Posts.SingleOrDefaultAsync(x => x.Id == request.PostId);
        if (post == null)
            throw new Exception( "Post Not Found Or Invalid Post ID");

        var profile = await _context.Profiles.SingleOrDefaultAsync(x => x.Id == request.ProfileId);
        if (profile == null)
            throw new Exception( "User Not Found Or Invalid User ID");

        var comment = await _context.Comments.SingleOrDefaultAsync(x => x.Id == request.CommentId);
        if (comment == null)
            throw new Exception( "Comment Not Found Or Invalid Comment ID");

        var commentLike = new CommentLikes()
        {
            CommentId = request.CommentId,
            ProfileId = request.ProfileId,
            ReactionType = request.ReactionType,

        };

        comment.ReactCount++;
        await _context.CommentLikes.AddAsync(commentLike);
        var likeOperation = await _context.SaveChangesAsync();
        var notificationRequest = new NotificationRequest()
        {
            RecipientId = profile.UserId,
            SenderId = profile.Id,
            Type = NotificationType.Like,
            Message = $"{profile.FullName} liked your post",
            ReferenceId = post.Id
        };
        await _notificationService.SendNotificationAsync(notificationRequest);
        return _mapper.Map<CommentResponse>(comment);
    }
}