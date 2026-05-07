using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SocialMedia.Application.Abstractions.PostAbstractions;
using SocialMedia.Application.DTOs.Responses.Posts;
using SocialMedia.Core.Context;
using SocialMedia.Core.Domain.DTOs.Requests.Share;

namespace SocialMedia.Application.Implementations;
public class ShareService(AppdbContext _context,IMapper _mapper) :  ISharePostService
{
    public async ValueTask<string> Revoke(RevokeShareRequest revoke)
    {
        var share = await _context.Shares.SingleOrDefaultAsync(x => x.Id == revoke.Id);
        if (share == null)
            return "Share Not Found";

        var profile = await _context.Profiles.SingleOrDefaultAsync(x => x.Id == revoke.ProfileId);
        if (profile == null)
            return "User Not Found";

        var _post = await _context.Posts.SingleOrDefaultAsync(x => x.Id == revoke.PostId);
        if (_post == null)
            return "Post Not Found";

        _post.ShareCount--;
        profile.Posts.Remove(_post);
        _context.Shares.Remove(share);
        var revokeOperation = await _context.SaveChangesAsync();

        return revokeOperation > 0 ?
            "Successfully" : "Invalid Revoke";
    }

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
