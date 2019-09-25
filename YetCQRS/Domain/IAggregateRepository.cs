using System;

namespace YetCQRS.Domain
{
    public interface IAggregateRepository<T> where T : AggregateRoot, new()
    {
        void Save(T aggregate, int? expectedVersion = null);
        T Get(Guid aggregateId);
    }
}
