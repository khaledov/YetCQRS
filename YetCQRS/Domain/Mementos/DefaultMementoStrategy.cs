using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using YetCQRS.EventStore;

namespace YetCQRS.Domain.Mementos
{
    public class DefaultMementoStrategy : IMementoStrategy
    {
        public bool IsMementable(Type aggregateType)
        {
            if (aggregateType.GetTypeInfo().BaseType == null)
                return false;
            if (aggregateType.GetTypeInfo().BaseType.GetTypeInfo().IsGenericType &&
                aggregateType.GetTypeInfo().BaseType.GetGenericTypeDefinition() == typeof(Memento))
                return true;
            return IsMementable(aggregateType.GetTypeInfo().BaseType);
        }

        public bool ShouldTakeMemento(AggregateRoot aggregate)
        {
            if (!IsMementable(aggregate.GetType())) return false;

            var aggregateVersion = aggregate.Version;
            if (aggregateVersion < 2) return false;
            IDomainEventProvider domainEventProvider = (IDomainEventProvider)aggregate;
            var unCommitedChangesCount = domainEventProvider.GetUncommittedChanges().ToList().Count;

            for (var j = 0; j < unCommitedChangesCount; j++)
                if (++aggregateVersion % 3 == 0 && aggregateVersion != 0)
                    return true;
            return false;

        }
    }
}
