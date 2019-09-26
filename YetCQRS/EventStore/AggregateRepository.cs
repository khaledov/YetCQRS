using System;
using System.Linq;
using System.Threading.Tasks;
using YetCQRS.Domain;
using YetCQRS.Domain.Exceptions;
using YetCQRS.Events;

namespace YetCQRS.EventStore
{
    public class AggregateRepository<T> : 
        IAggregateRepository<T> where T:AggregateRoot,new()
     {
        private readonly IEventStore _eventStore;
        private readonly IEventBus _eventBus;

        public AggregateRepository(IEventStore eventStore, IEventBus eventBus)
        {
            if (eventStore == null)
                throw new ArgumentNullException("eventStore");
            if (eventBus == null)
                throw new ArgumentNullException("eventBus");
            _eventStore = eventStore;
            _eventBus = eventBus;
        }

        public void Save(T aggregate, int? expectedVersion = null) 
        {
            if (expectedVersion != null && _eventStore.Get(
                    aggregate.Id, expectedVersion.Value).Any())
                throw new ConcurrencyException(aggregate.Id);
            var i = 0;
            IDomainEventProvider domainEventProvider= (IDomainEventProvider)aggregate;
            foreach (var @event in domainEventProvider.GetUncommittedChanges())
            {
                if (@event.Id == Guid.Empty)
                    @event.Id = aggregate.Id;
                if (@event.Id == Guid.Empty)
                    throw new AggregateOrEventMissingIdException(
                        aggregate.GetType(), @event.GetType());
                i++;
                @event.AggregateId = aggregate.Id;
                @event.Version = aggregate.Version + i;
                @event.TimeStamp = DateTimeOffset.UtcNow;
                _eventStore.Save(@event);
                _eventBus.Publish(aggregate.Id,@event);
            }
            domainEventProvider.MarkChangesAsCommitted();
        }

        public Task<T> Get(Guid aggregateId) 
        {
            return LoadAggregate(aggregateId);
        }

        private Task<T> LoadAggregate(Guid id) 
        {
          return  Task.Run(() => {
                var aggregate = new T();
                IDomainEventProvider domainEventProvider = (IDomainEventProvider)aggregate;
                var events = _eventStore.Get(id, -1);
                if (!events.Any())
                    throw new AggregateNotFoundException(id);

                domainEventProvider.LoadFromHistory(events);
              return aggregate;
          });
            
           
        }
    }
}
