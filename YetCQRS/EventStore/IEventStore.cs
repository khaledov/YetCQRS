using YetCQRS.Events;
using System;
using System.Collections.Generic;

namespace YetCQRS.EventStore
{
    public interface IEventStore
    {
        void Save(Event @event);
        IEnumerable<Event> Get(Guid aggregateId, int fromVersion);
    }
}
