using System;
using System.Linq;
using System.Threading.Tasks;
using YetCQRS.Domain;
using YetCQRS.Domain.Exceptions;
using YetCQRS.Events;
using YetCQRS.Thread;

namespace YetCQRS.EventStore
{
    public class AggregateRepository<T> : 
        IAggregateRepository<T> where T:AggregateRoot,new()
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
            if (expectedVersion != null && _eventStore.Get(
                    aggregate.Id, expectedVersion.Value).Any())
                throw new ConcurrencyException(aggregate.Id);

          
            IDomainEventProvider domainEventProvider= (IDomainEventProvider)aggregate;
            foreach (var @event in domainEventProvider.GetUncommittedChanges())
            {
                if (@event.Id == Guid.Empty)
                    @event.Id = aggregate.Id;
                if (@event.Id == Guid.Empty)
                    throw new AggregateOrEventMissingIdException(
                        aggregate.GetType(), @event.GetType());
             
                _eventStore.Save(aggregate.Id, @event);
               
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
