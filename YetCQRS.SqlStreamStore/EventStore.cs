using System;
using System.Collections.Generic;
using YetCQRS.Events;
using YetCQRS.EventStore;

namespace YetCQRS.SqlStreamStore
{
    public class EventStore : IEventStore
    {
        public IEventBus EventBus { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IEnumerable<Event> Get(Guid aggregateId, int fromVersion)
        {
            throw new NotImplementedException();
        }

        public void Save(Guid aggregateId, Event @event)
        {
            throw new NotImplementedException();
        }
    }
}
