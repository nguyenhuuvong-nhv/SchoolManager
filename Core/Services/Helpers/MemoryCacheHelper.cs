using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helpers
{
    public static class MemoryCacheHelper
    {
        private static MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

        public static object GetValue(string key)
        {
            return _cache.Get(key);
        }

        public static Task<TItem> GetOrCreateAsync<TItem>(object key, Func<ICacheEntry, Task<TItem>> factory)
            => _cache.GetOrCreateAsync(key, factory);

        public static TItem GetOrCreate<TItem>(object key, Func<ICacheEntry, TItem> factory)
            => _cache.GetOrCreate(key, factory);

        public static void Remove(object key)
            => _cache.Remove(key);

        public static object Get(object key)
            => _cache.Get(key);

        public static TItem Get<TItem>(object key)
            => _cache.Get<TItem>(key);

        public static TItem Set<TItem>(object key, TItem value)
            => _cache.Set(key, value);

        public static TItem Set<TItem>(object key, TItem value, MemoryCacheEntryOptions options)
            => _cache.Set(key, value, options);

        public static TItem Set<TItem>(object key, TItem value, IChangeToken expirationToken)
            => _cache.Set(key, value, expirationToken);

        public static TItem Set<TItem>(object key, TItem value, DateTimeOffset absoluteExpiration)
            => _cache.Set(key, value, absoluteExpiration);

        public static TItem Set<TItem>(object key, TItem value, TimeSpan absoluteExpirationRelativeToNow)
            => _cache.Set(key, value, absoluteExpirationRelativeToNow);
    }
}
