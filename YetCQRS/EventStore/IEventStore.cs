using YetCQRS.Events;
using System;
using System.Collections.Generic;
using System.Collections;

namespace YetCQRS.EventStore
{
    public interface IEventStore
    {
        IEventBus EventBus { get; set; }
        void Save(Guid aggregateId, Event @event);
        IEnumerable LoadEventsFor(Guid aggregateId, int fromVersion);
        //void SaveEventsFor(Guid id, int eventsLoaded, ArrayList newEvents);
    }
}
