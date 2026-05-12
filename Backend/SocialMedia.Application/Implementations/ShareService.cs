using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.Helpers;
namespace SocialMedia.Application.Implementations;
public class ShareService(AppdbContext _context,IMapper _mapper) :  ISharePostService
{
  
    public async ValueTask<string?> Start(StartShareRequest start)
    {
        var post = await _context.Posts.SingleOrDefaultAsync(x => x.Id == start.PostId);
        if (post == null)
            return null;

        var profile = await _context.Profiles.SingleOrDefaultAsync(x => x.Id == start.ProfileId);
        if (profile == null)
            return null;
        var token = Guid.NewGuid().ToString("N");
        var share = new Share()
        {
            PostId = start.PostId,
            CreatedAt = DateTime.UtcNow,
            ProfileId = start.ProfileId,
            ShareToken = token
        };
        _context.Shares.Add(share);
        post.ShareCount++;
        _context.Posts.Update(post);
         await _context.SaveChangesAsync();

        var url = $"https://localhost:7042/api/Share/share/{token}";

        return url;
    }
    public async ValueTask Revoke(RevokeShareRequest revoke)
    {
        var share = await _context.Shares.SingleOrDefaultAsync(x => x.Id == revoke.Id);
        if (share == null)
            throw new NotFoundException("Share not Found");

        var profile = await _context.Profiles.SingleOrDefaultAsync(x => x.Id == revoke.ProfileId);
        if (profile == null)
            throw new NotFoundException("User not Found");

        var post = await _context.Posts.SingleOrDefaultAsync(x => x.Id == revoke.PostId);
        if (post == null)
            throw new NotFoundException("Post not Found");

        post.ShareCount--;
        profile.Posts.Remove(post);
        _context.Shares.Remove(share);
        var revokeOperation = await _context.SaveChangesAsync();

        if( revokeOperation <=0) throw new BadRequestException("Invalid Revoke");
    }
    public async Task<PostResponse?> OpenSharedPost(string token)
    {
        var share = await _context.Shares
            .Include(x => x.Post)
            .FirstOrDefaultAsync(x => x.ShareToken == token);

        if (share == null)
            return null;

        return _mapper.Map<PostResponse>(share.Post);
    }
}
