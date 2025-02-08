using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using SagarImitation.Common.CacheSettings;
using SagarImitation.Model.Settings;

public class CustomCacheProvider : ICacheProvider
{
    private readonly IMemoryCache _cache;
    private readonly AppSettings _appSettings;
    private HashSet<string> _keys = new HashSet<string>();

    public CustomCacheProvider(IMemoryCache cache,IOptions<AppSettings> appSettings)
    {
        _cache = cache;
        _appSettings = appSettings.Value;
    }

    public void Set<TItem>(string key, TItem item, MemoryCacheEntryOptions options)
    {
        if (_appSettings.Caching)
        {
            _cache.Set(key, item, options);
            _keys.Add(key);
        }
    }

    public bool TryGetValue<TItem>(string key, out TItem value)
    {
        if (_appSettings.Caching)
        {
            return _cache.TryGetValue(key, out value);
        }
        else
        {
            value = default;
            return false;
        }
    }

    public void Remove(string key)
    {
        _cache.Remove(key);
        _keys.Remove(key);
    }

    public void RemoveByPattern(params string[] patterns)
    {
        var keysToRemove = _keys.Where(k => patterns.Any(p => k.Contains(p))).ToList();
        foreach (var key in keysToRemove)
        {
            Remove(key);
        }
    }
}