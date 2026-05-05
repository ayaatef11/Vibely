using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Context;
using SocialMedia.Core.Domain.DTOs.Requests.Like;

namespace SocialMedia.Application.Implementations;
public class PostLikeService(AppdbContext _context) :   IPostLikeService
{
    public async ValueTask<string> DisLikeAsync(DisLikeDTO like)
    {
        var _like = await _context.PostLike.FirstOrDefaultAsync(x => x.PostId == like.PostId && x.ProfileId ==like.ProfileId);
        if (_like == null)
            return "LikeNotFound";

        var _post = await _context.Posts.FirstOrDefaultAsync(x => x.Id == like.PostId);
        if (_post == null)
            return "PostNotFound";

        _post.ReactsCount--;
        _context.PostLike.Remove(_like);
        var dislikeOperation = await _context.SaveChangesAsync();

        return dislikeOperation > 0 ?
            "Successfully" : "Invalid";
    }

    public async ValueTask<string> LikeAsync(LikeDTO like)
    {
        var _react = new PostLikes()
        {
            PostId = like.PostId,
            ReactionType = like.React,
            ProfileId = like.ProfileId
        };

        var _post = await _context.Posts.FirstOrDefaultAsync(x => x.Id == like.PostId);
        if (_post == null)
            return "NotFound";

        _post.ReactsCount++;
        await _context.PostLike.AddAsync(_react);
        var likeOperation = await _context.SaveChangesAsync();

        return likeOperation > 0 ?"Successfully": "Invalid Add Like";
    }
}