using SocialMedia.Core.Domain.DTOs.Requests.Block;

namespace SocialMedia.Application.Abstractions;
public interface IBlockService  
{
    ValueTask BlockAsync(BlockRequest block);
    ValueTask UnBlockAsync(BlockRequest block);
    ValueTask<IEnumerable<User>> GetBlockedUserAsync(Guid id);
}