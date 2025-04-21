using System;
using System.Collections.Generic;
using Xunit;
using JITDispatcher.Domain;
using JITDispatcher.Events;
using JITDispatcher.Domain.Exceptions;
using Moq;
using JITDispatcher.EventStore;

namespace JITDispatcher.Tests
{
    public class AggregateRootTests
    {
        private class TestAggregateRoot : AggregateRoot
        {
            public TestAggregateRoot(Guid id) : base(id) { }
            public TestAggregateRoot() : base() { }

            public void ApplyTestEvent(IEvent @event)
            {
                ApplyChange(@event);
            }
        }

        [Fact]
        public void GetUncommittedChanges_ShouldReturnChanges()
        {
            var aggregate = new TestAggregateRoot();
            var @event = new Mock<IEvent>().Object;

            aggregate.ApplyTestEvent(@event);

            var changes = ((IDomainEventProvider)aggregate).GetUncommittedChanges();

            Assert.Contains(@event, changes);
        }

        [Fact]
        public void LoadFromHistory_ShouldApplyEvents()
        {
            var aggregate = new TestAggregateRoot();
            var @event = new Mock<IEvent>().Object;

            ((IDomainEventProvider)aggregate).LoadFromHistory(new List<IEvent> { @event });

            Assert.Equal(1, aggregate.Version);
        }

        [Fact]
        public void LoadFromHistory_ShouldThrowException_WhenEventsOutOfOrder()
        {
            var aggregate = new TestAggregateRoot();
            var @event = new Mock<IEvent>();
            @event.Setup(e => e.Version).Returns(2);

            Assert.Throws<EventsOutOfOrderException>(() => ((IDomainEventProvider)aggregate).LoadFromHistory(new List<IEvent> { @event.Object }));
        }

        [Fact]
        public void MarkChangesAsCommitted_ShouldClearChanges()
        {
            var aggregate = new TestAggregateRoot();
            var @event = new Mock<IEvent>().Object;

            aggregate.ApplyTestEvent(@event);
            ((IDomainEventProvider)aggregate).MarkChangesAsCommitted();

            var changes = ((IDomainEventProvider)aggregate).GetUncommittedChanges();

            Assert.Empty(changes);
        }
    }
}
