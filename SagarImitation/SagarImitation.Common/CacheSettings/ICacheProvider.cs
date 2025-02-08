using Microsoft.Extensions.Caching.Memory;

namespace SagarImitation.Common.CacheSettings
{
    public interface ICacheProvider
    {
        void Set<TItem>(string key, TItem item, MemoryCacheEntryOptions options);
        bool TryGetValue<TItem>(string key, out TItem value);
        void Remove(string key);
        void RemoveByPattern(params string[] patterns);
    }
}