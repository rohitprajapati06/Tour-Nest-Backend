namespace TourNest.Services.MemoryCache
{
    public interface IMemoryCacheService
    {
        void Set<T>(string key, T value, TimeSpan expiration);
        T Get<T>(string key);
        void Remove(string key);
    }
}
