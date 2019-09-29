using YetCQRS.Events;
using System;
using System.Collections.Generic;

namespace YetCQRS.EventStore
{
    public interface IEventStore
    {
        IEventBus EventBus { get; set; }
        void Save(Guid aggregateId, Event @event);
        IEnumerable<Event> Get(Guid aggregateId, int fromVersion);
    }
}
