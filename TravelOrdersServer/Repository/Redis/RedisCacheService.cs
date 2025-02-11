using Interface.Redis;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Repository.Redis;

public class RedisCacheService(IDistributedCache cache) : IRedisCacheService
{
    public async Task<T?> GetCachedDataAsync<T>(string key)
    {
        var jsonData = await cache.GetStringAsync(key);
        
        return jsonData is null ? default : JsonConvert.DeserializeObject<T>(jsonData);
    }

    public async Task SetCachedDataAsync<T>(string key, T data, TimeSpan cacheDuration)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = cacheDuration
        };

        var jsonData = JsonConvert.SerializeObject(data);
        await cache.SetStringAsync(key, jsonData, options);
    }

    public async Task RemoveCachedDataAsync(string key)
    {
        await cache.RemoveAsync(key);
    }
}