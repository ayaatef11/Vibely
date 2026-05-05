using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SocialMedia.Core.Context;
using SocialMedia.Core.Domain.DTOs.Requests.Block;

namespace SocialMedia.Application.Implementations;
public class BlockService : MainRepository<Block>, IBlockService
{
    private readonly AppdbContext context;
    private readonly IConfiguration config;
    public BlockService(AppdbContext context, IConfiguration config) : base(context, config)
    {
        this.context = context;
        this.config = config;
    }

    public async ValueTask<string> BlockAsync(BlockDTO block)
    {
        var foundBlock = await context.Blocks.SingleOrDefaultAsync
            (x => x.BlockerId == block.BlockerId && x.BlockedId == block.BlockedId);
        var foundBlocked = await context.Users.SingleOrDefaultAsync(x => x.Id == block.BlockedId);
        var foundBlocker = await context.Users.SingleOrDefaultAsync(x => x.Id == block.BlockerId);

        if (foundBlocked == null || foundBlocker == null)
            return "UserFF";
        if (foundBlock != null)
            return "UserAB";

        if (block.BlockerId == block.BlockedId) return "UserAA";


        var _block = new Block()
        {
            BlockedId = block.BlockedId,
            BlockerId = block.BlockerId
        };

        await context.Blocks.AddAsync(_block);
        var blockOperation = await context.SaveChangesAsync();
        return blockOperation > 0 ?
            "Successfully" : "Invalid";
    }

    public async ValueTask<string> UnBlockAsync(BlockDTO block)
    {
        var blocked = await context.Blocks.
            SingleOrDefaultAsync(x => x.BlockedId == block.BlockedId && x.BlockerId == block.BlockerId);

        if (blocked == null)
            return "UserNB";

        context.Blocks.Remove(blocked);
        var unBlockOperation = await context.SaveChangesAsync();

        return unBlockOperation > 0 ?
              "Successfully" : "Invalid";
    }

    public async ValueTask<IEnumerable<User>> GetBlockedUserAsync(Guid id)
    {
        return await context.Blocks.
            Where(x => x.BlockerId.ToString() == id.ToString().ToLower()). 
            Select(x => x.Blocked).
            ToListAsync();
    }
}