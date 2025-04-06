using System.Collections;
using YetCQRS.Events;

namespace YetCQRS.EventStore
{
    public interface IEventStore
    {
        IEventBus EventBus { get; set; }
        void Save(Guid aggregateId, IList<IEvent> newEvents);
        IEnumerable LoadEventsFor(Guid aggregateId, int fromVersion);

    }
}
