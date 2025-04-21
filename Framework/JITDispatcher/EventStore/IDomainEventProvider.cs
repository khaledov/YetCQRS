using System.Collections;
using JITDispatcher.Events;

namespace JITDispatcher.EventStore
{
    public interface IDomainEventProvider
    {
        IEnumerable<IEvent> GetUncommittedChanges();
        void LoadFromHistory(IEnumerable history);
        void MarkChangesAsCommitted();


    }
}
