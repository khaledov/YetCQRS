using System.Collections;
using YetCQRS.Events;

namespace YetCQRS.EventStore
{
    public interface IDomainEventProvider
    {
        IEnumerable<IEvent> GetUncommittedChanges();
        void LoadFromHistory(IEnumerable history);
        void MarkChangesAsCommitted();


    }
}
