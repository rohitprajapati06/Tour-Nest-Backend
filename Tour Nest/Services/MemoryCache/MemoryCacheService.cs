using Microsoft.Extensions.Caching.Memory;

namespace TourNest.Services.MemoryCache;

public class MemoryCacheService(IMemoryCache memoryCache) : IMemoryCacheService
{
    private readonly IMemoryCache _memoryCache = memoryCache;

    public void Set<T>(string key, T value, TimeSpan duration)
    {
        _memoryCache.Set(key, value, duration);
    }

    public T? Get<T>(string key)
    {
        return _memoryCache.TryGetValue(key, out T value) ? value : default;
    }

    public void Remove(string key)
    {
        _memoryCache.Remove(key);
    }
}

