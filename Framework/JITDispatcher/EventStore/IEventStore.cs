using System.Collections;
using JITDispatcher.Events;

namespace JITDispatcher.EventStore
{
    public interface IEventStore
    {
        IEventBus EventBus { get; set; }
        void Save(Guid aggregateId, IList<IEvent> newEvents);
        IEnumerable LoadEventsFor(Guid aggregateId, int fromVersion);

    }
}
