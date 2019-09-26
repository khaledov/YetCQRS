using System;
using System.Collections.Generic;
using System.Text;
using YetCQRS.Domain;

namespace YetCQRS.Cache
{
    public interface ICache
    {
        void Set(AggregateRoot aggregateRoot);
        AggregateRoot Get(Guid aggregateRootId);
        void Remove(Guid aggregateRootId);
        void RegisterEvictionCallback(Action<Guid> action);
    }
}
