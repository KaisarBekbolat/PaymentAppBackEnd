using System;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace PaymentProject.Services.Redis;

public class RedisCacheService : IRedisCacheService
{
    private IDistributedCache distributedCache;

    public RedisCacheService(IDistributedCache cache){
        distributedCache = cache;
    }
    public T? GetData<T>(string key)
    {
        var data = distributedCache.GetString(key);
        if(data is null){
            return default(T);
        }
        return JsonSerializer.Deserialize<T>(data);
    }

    public void SetData<T>(string key, T data)
    {
        var options = new DistributedCacheEntryOptions(){
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(3)
        };
        distributedCache.SetString(key, JsonSerializer.Serialize<T>(data), options);
    }
}
