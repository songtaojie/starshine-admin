using Hx.Cache;

namespace Hx.Admin.Core;

/// <summary>
/// SqlSugar二级缓存
/// </summary>
public class SqlSugarCache : ICacheService
{
    private readonly ICache _cache;

    public SqlSugarCache(ICache cache)
    {
        _cache = cache;
    }

    public void Add<V>(string key, V value)
    {
        _cache.Set(key, value);
    }

    public void Add<V>(string key, V value, int cacheDurationInSeconds)
    {
        _cache.Set(key, value, cacheDurationInSeconds);
    }

    public bool ContainsKey<V>(string key)
    {
        return _cache.ExistsKey(key);
    }

    public V Get<V>(string key)
    {
        return _cache.Get<V>(key);
    }

    public IEnumerable<string> GetAllKey<V>()
    {
        return _cache.GetAllKeys();
    }

    public V GetOrCreate<V>(string cacheKey, Func<V> create, int cacheDurationInSeconds = int.MaxValue)
    {
        if (!_cache.ExistsKey(cacheKey))
        {
            var value = create();
            if (cacheDurationInSeconds <= 0)
            {
                _cache.Set(cacheKey, value);
            }
            else
            {
                _cache.Set(cacheKey, value, cacheDurationInSeconds);
            }
            return value;
        }
        return _cache.Get<V>(cacheKey);
    }

    public void Remove<V>(string key)
    {
        _cache.Remove(key);
    }
}