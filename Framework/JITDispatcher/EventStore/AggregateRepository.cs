using JITDispatcher.Domain;
using JITDispatcher.Domain.Exceptions;
using JITDispatcher.Thread;

namespace JITDispatcher.EventStore
{
    public class AggregateRepository<T> :
        IAggregateRepository<T> where T : AggregateRoot, new()
    {
        private readonly IEventStore _eventStore;
        private NamedLocker _locker = new NamedLocker();

        public AggregateRepository(IEventStore eventStore)
        {
            if (eventStore == null)
                throw new ArgumentNullException("eventStore");

            _eventStore = eventStore;

        }

        public void Save(T aggregate, int? expectedVersion = null)
        {
            lock (_locker.GetLock(aggregate.Id.ToString()))
            {
                IDomainEventProvider domainEventProvider = (IDomainEventProvider)aggregate;
                var eventList = domainEventProvider.GetUncommittedChanges().ToList();
                eventList.ForEach(e =>
                {
                    if (e.Id == Guid.Empty)
                        e.Id = aggregate.Id;
                    if (e.Id == Guid.Empty)
                        throw new AggregateOrEventMissingIdException(
                            aggregate.GetType(), e.GetType());
                });

                _eventStore.Save(aggregate.Id, eventList);
                domainEventProvider.MarkChangesAsCommitted();
            }


        }

        public Task<T> Get(Guid aggregateId)
        {
            return LoadAggregate(aggregateId);
        }

        private Task<T> LoadAggregate(Guid id)
        {
            return Task.Run(() =>
            {
                var aggregate = new T();
                IDomainEventProvider domainEventProvider = (IDomainEventProvider)aggregate;
                var events = _eventStore.LoadEventsFor(id, -1);

                domainEventProvider.LoadFromHistory(events);
                return aggregate;
            });


        }
    }
}
