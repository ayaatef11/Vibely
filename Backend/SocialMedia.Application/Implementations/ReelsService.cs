using AutoMapper;
using Microsoft.EntityFrameworkCore; 
 
namespace SocialMedia.Application.Implementations;
public class ReelsService(AppdbContext context, IMapper _mapper) : IReelsService
{
    public async ValueTask<IEnumerable<ReelsResponse>> GetUserRealsAsync(Guid userId)
    {
        var reels = await context.Reels.Where(x => x.UserId == userId).OrderByDescending(x => x.UploadAt).ToListAsync();
        return _mapper.Map<List<ReelsResponse>>(reels);
    }

    public async ValueTask<string> RemoveAsync(Guid realId)
    {
        var real = await context.Reels.SingleOrDefaultAsync(x => x.Id == realId);
        if (real == null)
            throw new Exception("Real Not Found or Invalid Real Id");

        context.Reels.Remove(real);
        var deleteOperation = await context.SaveChangesAsync();
        return deleteOperation > 0 ? "Successfully" : "Failed To Delete Real";
    }

    public async ValueTask<string> UploadAsync(ReelsRequest request)
    {
        var user = await context.Users.SingleOrDefaultAsync(x => x.Id == request.UserId);
        if (user == null) throw new Exception("User Not Found Or Invalid User Id");

        var Real = new Reels()
        {
            UserId = request.UserId,
            UploadAt = DateTime.UtcNow,
        };

        // upload file
        using var videoMemoryStreem = new MemoryStream();
        await request.file?.CopyToAsync(videoMemoryStreem);
        Real.ContentType = request.file?.ContentType;
        Real.Data = videoMemoryStreem.ToArray();
        Real.FileName = request.file?.FileName;

        await context.Reels.AddAsync(Real);
        var addOperation = await context.SaveChangesAsync();

        return addOperation > 0 ? "Successfully" : "Failed To Upload Real";
    }
}
  
