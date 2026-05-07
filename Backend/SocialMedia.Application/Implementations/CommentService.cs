using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.DTOs.Responses;
using SocialMedia.Core.Context;
using SocialMedia.Core.Domain.DTOs.Requests.Comment;

namespace SocialMedia.Application.Implementations;
public class CommentService(AppdbContext _context,IMapper _mapper) :  ICommentService
{ 
    public async ValueTask<CommentResponse> AddComment(AddCommentRequest commentRequest)
    {

        var comment = new Comment()
        {
            ReactCount = 0,
            Text = commentRequest.Text,
            PostId = commentRequest.PostId,
            AddedAt = DateTime.UtcNow,
            ProfileId = commentRequest.ProfileId,
        };

        var post = await _context.Posts.Where(c=>c.Id==commentRequest.PostId).FirstAsync();
        post.CommentsCount++;
        _context.Posts.Update(post);

         await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();
        var profile=await _context.Profiles.Where(c=>c.Id== commentRequest.ProfileId).FirstAsync();
        var result = _mapper.Map<CommentResponse>(comment);
        result.UserName=profile.UserName;
        result.ProfileImage=profile.ProfileImage;
        return result;
    }

    public async ValueTask<CommentResponse?> EditComment(EditCommentRequest commentRequest)
    {
        var _comment = await _context.Comments.Where(c=>c.Id== commentRequest.Id).Include(c=>c.Profile).FirstAsync();
        if (_comment == null)
            return null;

        _comment.Text = commentRequest.Text;
          _context.Comments.Update(_comment);
         await _context.SaveChangesAsync();
        var result = _mapper.Map<CommentResponse>(_comment);
        return result;
    }
    public async ValueTask<int> DeleteComment(Guid id)
    {
        var comment = await _context.Comments.FindAsync(id);
        if (comment == null) return -1;
        _context.Comments.Remove(comment);
        return await _context.SaveChangesAsync();
    }
    public async ValueTask<IEnumerable<CommentResponse>> GetComments(Guid postId)
    {
        var comments = await _context.Comments.Where(x => x.PostId == postId).Include(c=>c.Profile).ToListAsync();
        return _mapper.Map<IEnumerable<CommentResponse>>(comments);
    }


}

