using YetCQRS.Domain;

namespace YetCQRS.EventStore
{
    public interface IAggregateRepository<T> where T : AggregateRoot, new()
    {
        void Save(T aggregate, int? expectedVersion = null);
        Task<T> Get(Guid aggregateId);
    }
}
