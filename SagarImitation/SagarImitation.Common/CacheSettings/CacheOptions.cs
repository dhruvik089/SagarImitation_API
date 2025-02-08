using Microsoft.Extensions.Caching.Memory;

namespace SagarImitation.Common.Common.CacheSettings
{
    public class CacheOptions
    {
        public static MemoryCacheEntryOptions cacheEntryOption = new MemoryCacheEntryOptions
        {
            AbsoluteExpiration = DateTime.Now.AddMinutes(30),
            SlidingExpiration = TimeSpan.FromMinutes(30),
            Size = 1024 * 3 // 3MB
        };
    }
}