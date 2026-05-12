using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.Helpers;

namespace SocialMedia.Application.Implementations;
public class PostLikeService(AppdbContext _context) : IPostLikeService
{
    public async ValueTask DisLikeAsync(DisLikeRequest request)
    {
        var like = await _context.PostLike.FirstOrDefaultAsync(x => x.PostId == request.PostId && x.ProfileId == request.ProfileId);
        if (like == null)
            throw new NotFoundException( "LikeNotFound");

        var post = await _context.Posts.FirstOrDefaultAsync(x => x.Id == request.PostId);
        if (post == null)
            throw new NotFoundException("PostNotFound");

        post.ReactsCount--;
        _context.PostLike.Remove(like);
        var dislikeOperation = await _context.SaveChangesAsync();

        if( dislikeOperation <= 0) throw new BadRequestException("Invalid");
    }

    public async ValueTask LikeAsync(LikeRequest like)
    {
        var react = new PostLikes()
        {
            PostId = like.PostId,
            ReactionType = like.React,
            ProfileId = like.ProfileId
        };

        var post = await _context.Posts.FirstOrDefaultAsync(x => x.Id == like.PostId);
        if (post == null)
            throw new NotFoundException("Not Found");

        post.ReactsCount++;
        await _context.PostLike.AddAsync(react);
        var likeOperation = await _context.SaveChangesAsync();

        if( likeOperation <= 0) throw new BadRequestException("Invalid Add Like");
    }
}