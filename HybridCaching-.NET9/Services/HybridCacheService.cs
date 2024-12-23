using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace HybridCaching.Services
{
    public class HybridCacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IDistributedCache _distributedCache;

        public HybridCacheService(IMemoryCache memoryCache, IDistributedCache distributedCache)
        {
            _memoryCache = memoryCache;
            _distributedCache = distributedCache;
        }

        // Get cache data from MemoryCache or fallback to Distributed Cache
        public async Task<string> GetCachedDataAsync(string key)
        {
            // Try to get data from MemoryCache
            if (_memoryCache.TryGetValue(key, out string cachedData))
            {
                return cachedData;
            }

            // If not in MemoryCache, try to get it from Distributed Cache
            cachedData = await _distributedCache.GetStringAsync(key);
            if (cachedData != null)
            {
                // Store in MemoryCache for quicker access next time
                _memoryCache.Set(key, cachedData, TimeSpan.FromMinutes(5));
            }

            return cachedData;
        }

        // Set cache data in both MemoryCache and Distributed Cache
        public async Task SetCacheDataAsync(string key, string data)
        {
            // Store in MemoryCache
            _memoryCache.Set(key, data, TimeSpan.FromMinutes(5));

            // Store in Distributed Cache (SQL Server)
            await _distributedCache.SetStringAsync(key, data, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) // Adjust expiration as needed
            });
        }
    }

}
