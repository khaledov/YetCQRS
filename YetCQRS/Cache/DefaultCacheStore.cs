using Microsoft.Extensions.Caching.Memory;
using YetCQRS.Domain;

namespace YetCQRS.Cache
{
    public class DefaultCacheStore : ICache
    {
        private readonly MemoryCacheEntryOptions _cacheOptions;
        private readonly IMemoryCache _cache;

        public DefaultCacheStore()
        {
            _cacheOptions = new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(15)
            };
            _cache = new MemoryCache(new MemoryCacheOptions());
        }

        public AggregateRoot? Get(Guid aggregateRootId) => _cache?.Get<AggregateRoot>(aggregateRootId);

        public void RegisterEvictionCallback(Action<Guid> action)
        {
            _cacheOptions.RegisterPostEvictionCallback((key, value, reason, state) =>
            {
                if (key is Guid id)
                {
                    action.Invoke(id);
                }
            });
        }

        public void Remove(Guid aggregateRootId)
        {
            _cache.Remove(aggregateRootId);
        }

        public void Set(AggregateRoot aggregateRoot)
        {
            _cache.Set(aggregateRoot.Id, aggregateRoot, _cacheOptions);
        }

        public bool IsTracked(Guid id)
        {
            return _cache.TryGetValue(id, out _);
        }
    }
}
