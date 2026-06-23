using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.BIL.Services.CacheService
{
    public  interface ICacheService
    {
        Task SetAsync<T>(string key, T Value, int Minutes);
        Task<T?> GetAsync<T>(string Key);
        Task RemoveAsync(string Key);
        Task RefreshVersionAsync(string entity);
        Task<string> GetVersionAsync(string entity);
    }
}
