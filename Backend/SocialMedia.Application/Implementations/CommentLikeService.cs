using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SocialMedia.Application.DTOs.Responses;
using SocialMedia.Core.Context;
using SocialMedia.Core.Domain.DTOs.Requests.Comment;

namespace SocialMedia.Application.Implementations;
public class CommentLikeService(AppdbContext _context,IMapper _mapper) :  ICommentLikeService
{
    public async ValueTask<CommentResponse> DisLikeAsync(LikeCommentDTO like)
    {
        var _like = await _context.CommentLikes.SingleOrDefaultAsync(x => x.CommentId == like.CommentId);
        if (_like == null)
            throw new Exception("Like Not Found Or Invalid Like ID");

        var post = await _context.Posts.SingleOrDefaultAsync(x => x.Id == like.PostId);
        if (post == null)
            throw new Exception("Post Not Found Or Invalid Post ID");

        var profile = await _context.Profiles.SingleOrDefaultAsync(x => x.Id == like.ProfileId);
        if (profile == null)
            throw new Exception("User Not Found Or Invalid User ID");

        var comment = await _context.Comments.SingleOrDefaultAsync(x => x.Id == like.CommentId);
        if (comment == null)
            throw new Exception("Comment Not Found Or Invalid Comment ID");

        comment.ReactCount--;
        _context.CommentLikes.Remove(_like);
        var dislikeOperation = await _context.SaveChangesAsync();
        return _mapper.Map<CommentResponse>(comment);
    }

    public async ValueTask<CommentResponse> LikeAsync(LikeCommentDTO like)
    {
        var post = await _context.Posts.SingleOrDefaultAsync(x => x.Id == like.PostId);
        if (post == null)
            throw new Exception( "Post Not Found Or Invalid Post ID");

        var profile = await _context.Profiles.SingleOrDefaultAsync(x => x.Id == like.ProfileId);
        if (profile == null)
            throw new Exception( "User Not Found Or Invalid User ID");

        var comment = await _context.Comments.SingleOrDefaultAsync(x => x.Id == like.CommentId);
        if (comment == null)
            throw new Exception( "Comment Not Found Or Invalid Comment ID");

        var commentLike = new CommentLikes()
        {
            CommentId = like.CommentId,
            ProfileId = like.ProfileId,
            ReactionType = like.ReactionType,

        };

        comment.ReactCount++;
        await _context.CommentLikes.AddAsync(commentLike);
        var likeOperation = await _context.SaveChangesAsync();
        return _mapper.Map<CommentResponse>(comment);
    }
}