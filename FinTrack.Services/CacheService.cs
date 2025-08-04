using FinTrack.Services.Contracts;
using Microsoft.Extensions.Caching.Distributed;

namespace FinTrack.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _cache;

        public CacheService(IDistributedCache cache)
        { 
            _cache = cache; 
        }

        #region private metods

        private async Task<string> GetCache(string cashKey)
        {
            var cash = await _cache.GetStringAsync(cashKey);
            return cash;
        }

        #endregion
    }
}
