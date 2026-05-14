using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.Helpers;
namespace SocialMedia.Application.Implementations;
public class SavePostService (AppdbContext _context,IMapper _mapper) :ISavePostService
{ 
    public async ValueTask<IEnumerable<PostResponse>> GetPosts(Guid userId)
    {
        var posts = await _context.Posts
          .Where(x => x.SaverIds != null)
          .ToListAsync();

        var postFiltered=posts.Where(x =>
        {
            var saverIds =JsonHelper.ConvertToList(x.SaverIds);

            return saverIds.Contains(userId);
        });
        var result = _mapper.Map<List<PostResponse>>(postFiltered);
        return result;
    }

    public async ValueTask SaveAsync(SavePostRequest savePost)
    {
        var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == savePost.UserId);
        if (user == null)
            throw new NotFoundException( "User Not Found Or Inviald User Id");

        var post = await _context.Posts.SingleOrDefaultAsync(x => x.Id == savePost.PostId);
        if (post == null)
            throw new NotFoundException("Post Not Found Or Inviald Post Id");
        var saverIds = JsonHelper.ConvertToList(post.SaverIds);

        if (!saverIds.Contains(savePost.UserId))
            saverIds.Add(savePost.UserId);

        post.SaverIds = JsonHelper.ConvertToString(saverIds);       

         _context.Posts.Update(post);
        var saveOperation = await _context.SaveChangesAsync();
        if( saveOperation <= 0 )throw new Exception("Failed To Save Post");
    }

    public async ValueTask UnSaveAsync(SavePostRequest savePost)
    {
        var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == savePost.UserId);
        if (user == null)
            throw new NotFoundException("User Not Found Or invalid User Id");

        var post = await _context.Posts.SingleOrDefaultAsync(x => x.Id == savePost.PostId);
        if (post == null)
            throw new NotFoundException("Post not Found or invalid Post Id");

        var saverIds = JsonHelper.ConvertToList(post.SaverIds);

        saverIds.Remove(savePost.UserId);

        post.SaverIds = JsonHelper.ConvertToString(saverIds);

        _context.Posts.Update(post);
        var deleteOperation = await _context.SaveChangesAsync();
        if( deleteOperation <=0 ) throw new Exception("Failed To unsave Post");
    }
}
