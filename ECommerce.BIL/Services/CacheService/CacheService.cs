using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Distributed;

namespace ECommerce.BIL.Services.CacheService
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        public CacheService(IDistributedCache cache)
        { 
           _cache = cache;
        }
        public async Task<T?> GetAsync<T>(string Key)
        {
           var data =  await _cache.GetStringAsync(Key);
            if (data == null) { 
            return default;
            }
            return JsonSerializer.Deserialize<T>(data);

        }

        public async Task RemoveAsync(string Key)
        {      
            await _cache.RemoveAsync(Key);
        }

        public async Task SetAsync<T>(string key, T Value, int Minutes)
        {
            await _cache.SetStringAsync(key, JsonSerializer.Serialize<T>(Value),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(Minutes)
                }
                );
        }
        public async Task<string>GetVersionAsync(string entity)
        {
            string Key = $"{entity}_version";
            var version =await _cache.GetStringAsync(Key);
            if (version == null)
            {
                version = Guid.NewGuid().ToString();
                await _cache.SetStringAsync(Key, version);
            }
            return version;

        }

        public async Task RefreshVersionAsync(string entity)
        {
            await _cache.SetStringAsync($"{entity}_version", Guid.NewGuid().ToString());
        }
    }
}
