using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.DTOs.Responses.Posts;
using SocialMedia.Core.Context;
using SocialMedia.Core.Domain.DTOs.Requests.SavePosts;

namespace SocialMedia.Application.Implementations;
public class SavePostService (AppdbContext _context,IMapper _mapper) :ISavePostService
{ 
    public async ValueTask<IEnumerable<PostResponse>> GetPosts(Guid userId)
    {
        var posts = await _context.Posts
          .Where(x => x.IsSaved == true && x.SaverIds != null)
          .ToListAsync();

        var postFiltered=posts.Where(x =>
        {
            var saverIds =JsonHelper.ConvertToList(x.SaverIds);

            return saverIds.Contains(userId);
        });
        var result = _mapper.Map<List<PostResponse>>(postFiltered);
        return result;
    }

    public async ValueTask<string> SaveAsync(SavePostDTO savePost)
    {
        var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == savePost.UserId);
        if (user == null)
            return "User Not Found Or Inviald User Id";

        var post = await _context.Posts.SingleOrDefaultAsync(x => x.Id == savePost.PostId);
        if (post == null)
            return "Post Not Found Or Inviald Post Id";
        var saverIds = JsonHelper.ConvertToList(post.SaverIds);

        if (!saverIds.Contains(savePost.UserId))
            saverIds.Add(savePost.UserId);

        post.SaverIds = JsonHelper.ConvertToString(saverIds);
        post.IsSaved = true;
       

         _context.Posts.Update(post);
        var saveOperation = await _context.SaveChangesAsync();
        return saveOperation > 0 ?
            "Successfully" :
            "Failed To Save Post";
    }

    public async ValueTask<string> UnSaveAsync(SavePostDTO savePost)
    {
        var user = await _context.Users.
          SingleOrDefaultAsync(x => x.Id == savePost.UserId);
        if (user == null)
            return "User Not Found Or Inviald User Id";

        var post = await _context.Posts
            .SingleOrDefaultAsync(x => x.Id == savePost.PostId);
        if (post == null)
            return "Post Not Found Or Inviald Post Id";

        var saverIds = JsonHelper.ConvertToList(post.SaverIds);

        saverIds.Remove(savePost.UserId);

        post.SaverIds = JsonHelper.ConvertToString(saverIds);

        if (saverIds.Count == 0)
            post.IsSaved = false;

        _context.Posts.Update(post);
        var deleteOperation = await _context.SaveChangesAsync();
        return deleteOperation > 0 ?
            "Successfully" :
            "Failed To UnSave Post";
    }
}
