using Microsoft.Extensions.Caching.Memory;
using TeamPortfolio.Application.Interfaces.Services;

namespace TeamPortfolio.Infrastructure.Services;

public class CacheService : ICacheService
{
    private readonly IMemoryCache _cache;
    private readonly HashSet<string> _keys = new();
    private readonly object _lock = new();

    public CacheService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public Task<T?> GetAsync<T>(string key)
    {
        _cache.TryGetValue(key, out T? value);
        return Task.FromResult(value);
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        var options = new MemoryCacheEntryOptions();
        if (expiry.HasValue) options.SetAbsoluteExpiration(expiry.Value);
        _cache.Set(key, value, options);
        lock (_lock) { _keys.Add(key); }
        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key)
    {
        _cache.Remove(key);
        lock (_lock) { _keys.Remove(key); }
        return Task.CompletedTask;
    }

    public Task RemoveByPrefixAsync(string prefix)
    {
        List<string> toRemove;
        lock (_lock) { toRemove = _keys.Where(k => k.StartsWith(prefix)).ToList(); }
        foreach (var key in toRemove)
        {
            _cache.Remove(key);
            lock (_lock) { _keys.Remove(key); }
        }
        return Task.CompletedTask;
    }
}
