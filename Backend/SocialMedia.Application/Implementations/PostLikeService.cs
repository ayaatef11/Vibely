using Microsoft.EntityFrameworkCore;

namespace SocialMedia.Application.Implementations;
public class PostLikeService(AppdbContext _context) : IPostLikeService
{
    public async ValueTask DisLikeAsync(DisLikeRequest like)
    {
        var _like = await _context.PostLike.FirstOrDefaultAsync(x => x.PostId == like.PostId && x.ProfileId ==like.ProfileId);
        if (_like == null)
            throw new Exception( "LikeNotFound");

        var _post = await _context.Posts.FirstOrDefaultAsync(x => x.Id == like.PostId);
        if (_post == null)
            throw new Exception("PostNotFound");

        _post.ReactsCount--;
        _context.PostLike.Remove(_like);
        var dislikeOperation = await _context.SaveChangesAsync();

        if( dislikeOperation <= 0) throw new Exception("Invalid");
    }

    public async ValueTask LikeAsync(LikeRequest like)
    {
        var _react = new PostLikes()
        {
            PostId = like.PostId,
            ReactionType = like.React,
            ProfileId = like.ProfileId
        };

        var _post = await _context.Posts.FirstOrDefaultAsync(x => x.Id == like.PostId);
        if (_post == null)
            throw new Exception("Not Found");

        _post.ReactsCount++;
        await _context.PostLike.AddAsync(_react);
        var likeOperation = await _context.SaveChangesAsync();

        if( likeOperation <= 0) throw new Exception("Invalid Add Like");
    }
}