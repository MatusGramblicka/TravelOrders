namespace Interface.Redis;

public interface IRedisCacheService
{
    Task<T?> GetCachedDataAsync<T>(string key);

    Task SetCachedDataAsync<T>(string key, T data, TimeSpan cacheDuration);

    Task RemoveCachedDataAsync(string key);
}