using YetCQRS.Domain.Exceptions;
using YetCQRS.Events;
using YetCQRS.Extensions;
using YetCQRS.Thread;
using System;
using System.Collections.Generic;
using YetCQRS.EventStore;
using YetCQRS.Domain.Mementos;
using System.Collections;

namespace YetCQRS.Domain
{
    public abstract class AggregateRoot :Entity<Guid>, IDomainEventProvider
    {
        public AggregateRoot(Guid id):
            base(id)
        {

        }
        public AggregateRoot():
            base(Guid.NewGuid())
        {

        }

        #region Attributes
        private readonly List<IEvent> _changes = new List<IEvent>();

        private NamedLocker _locker = new NamedLocker();

        public int Version { get; protected set; }

        #endregion
        #region Implemented Methods
        IEnumerable<IEvent> IDomainEventProvider.GetUncommittedChanges()
        {
            lock (_locker.GetLock(Id.ToString()))
            {
                return _changes;
            }
        }

        void IDomainEventProvider.LoadFromHistory(IEnumerable history)
        {
            lock (_locker.GetLock(Id.ToString()))
            {

                foreach (var e in history)
                {
                    var @event = e as IEvent;
                    if (@event.Version != Version + 1)
                        throw new EventsOutOfOrderException(@event.Id);
                    ApplyChange(@event, false);
                }
            }
        }

        void IDomainEventProvider.MarkChangesAsCommitted()
        {
            lock (_locker.GetLock(Id.ToString()))
            {
                Version = Version + _changes.Count;
                _changes.Clear();
            }
        }
        #endregion
        private void ApplyChange(IEvent @event, bool isNew)
        {
            lock (_locker.GetLock(Id.ToString()))
            {
                this.AsDynamic().Apply(@event);
                if (isNew) _changes.Add(@event);
                else
                {
                    Id = @event.Id;
                    Version++;
                }
            }
        }
        protected void ApplyChange(IEvent @event)
        {
            ApplyChange(@event, true);
        }
        
           
    }
}
    
