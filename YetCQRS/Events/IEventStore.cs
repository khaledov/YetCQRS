using YetCQRS.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YetCQRS.Events
{
    public interface IEventStore
    {
        Task<IEnumerable<Event>> GetEvents();
        Task<IEnumerable<Event>> GetEvents(Guid aggregateId);
        Task Save(AggregateRoot aggregateRoot);
    }
}
