using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SocialMedia.Application.Helpers;
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

    public async ValueTask BlockAsync(BlockRequest block)
    {
        var foundBlock = await context.Blocks.SingleOrDefaultAsync
            (x => x.BlockerId == block.BlockerId && x.BlockedId == block.BlockedId);
        var foundBlocked = await context.Users.SingleOrDefaultAsync(x => x.Id == block.BlockedId);
        var foundBlocker = await context.Users.SingleOrDefaultAsync(x => x.Id == block.BlockerId);

        if (foundBlocked == null || foundBlocker == null)
            throw new NotFoundException("user not found");
        if (foundBlock != null)
            throw new BadRequestException("user blocked before");

        if (block.BlockerId == block.BlockedId) throw new BadRequestException("user cant block themselves");


        var _block = new Block()
        {
            BlockedId = block.BlockedId,
            BlockerId = block.BlockerId
        };

        await context.Blocks.AddAsync(_block);
        var blockOperation = await context.SaveChangesAsync();
        if( blockOperation <= 0) throw new BadRequestException( "Invalid operation");
    }

    public async ValueTask UnBlockAsync(BlockRequest block)
    {
        var blocked = await context.Blocks.
            SingleOrDefaultAsync(x => x.BlockedId == block.BlockedId && x.BlockerId == block.BlockerId);

        if (blocked == null)
            throw new NotFoundException("UserNB");

        context.Blocks.Remove(blocked);
        var unBlockOperation = await context.SaveChangesAsync();

        if( unBlockOperation <= 0 )throw new BadRequestException( "Invalid");
    }

    public async ValueTask<IEnumerable<User>> GetBlockedUserAsync(Guid id)
    {
        return await context.Blocks.
            Where(x => x.BlockerId.ToString() == id.ToString().ToLower()). 
            Select(x => x.Blocked).
            ToListAsync();
    }
}