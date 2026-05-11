using Microsoft.Extensions.Caching.Memory;

namespace SocialMedia.Application.Implementations;
public class TokenBlocklistService(IMemoryCache _cache) : ITokenBlocklistService
{ 
    public void RevokeToken(string token, DateTime expiry)
    {
        if (expiry < DateTime.UtcNow) return;
        var ttl = expiry - DateTime.UtcNow;
        _cache.Set($"blocklist_{token}", true, ttl);
    }

    public bool IsRevoked(string token)
    {
        return _cache.TryGetValue($"blocklist_{token}", out _);
    }
}
