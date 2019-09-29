using System.Collections.Generic;
using YetCQRS.Events;

namespace YetCQRS.EventStore
{
    public interface IDomainEventProvider
    {
        IEnumerable<Event> GetUncommittedChanges();
        void LoadFromHistory(IEnumerable<Event> history);
        void MarkChangesAsCommitted();

       
    }
}
