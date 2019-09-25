using YetCQRS.Domain.Exceptions;
using YetCQRS.Events;
using YetCQRS.Extensions;
using YetCQRS.Thread;
using System;
using System.Collections.Generic;

namespace YetCQRS.Domain
{
    public abstract class AggregateRoot : IOriginator
    {
        #region Attributes
        private readonly List<Event> _changes = new List<Event>();
        public Guid Id { get; protected set; }
        public int Version { get; protected set; }

        private NamedLocker _locker = new NamedLocker();
        #endregion

        public IEnumerable<Event> GetUncommitedChanges()
        {
            lock (_locker.GetLock(Id.ToString()))
            {
                return _changes;
            }
        }
        public void MarkChangesAsCommited()
        {
            lock (_locker.GetLock(Id.ToString()))
            {
                Version = Version + _changes.Count;
                _changes.Clear();
            }
        }
        public void LoadFromHistory(IEnumerable<Event> history)
        {
            foreach (var e in history)
            {
                if (e.Version != Version + 1)
                    throw new EventsOutOfOrderException(e.Id);
                ApplyChange(e, false);
            }
        }
        private void ApplyChange(Event @event, bool isNew)
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
        protected void ApplyChange(Event @event)
        {
            ApplyChange(@event, true);
        }

        public abstract Memento GetMemento();

        public abstract void SetMemento(Memento memento);

    }
}
