using SocialMedia.Core.Domain.DTOs.Requests.Block;

namespace SocialMedia.Application.Abstractions;
public interface IBlockService  
{
    ValueTask<string> BlockAsync(BlockRequest block);
    ValueTask<string> UnBlockAsync(BlockRequest block);
    ValueTask<IEnumerable<User>> GetBlockedUserAsync(Guid id);
}