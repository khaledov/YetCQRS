using YetCQRS.Domain;

namespace YetCQRS.Cache
{
    public interface ICache
    {
        bool IsTracked(Guid id);
        void Set(AggregateRoot aggregateRoot);
        AggregateRoot? Get(Guid aggregateRootId);
        void Remove(Guid aggregateRootId);
        void RegisterEvictionCallback(Action<Guid> action);
    }
}
