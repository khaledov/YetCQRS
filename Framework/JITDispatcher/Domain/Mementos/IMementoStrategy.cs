using System;
using System.Collections.Generic;
using System.Text;

namespace JITDispatcher.Domain.Mementos
{
    public interface IMementoStrategy
    {
        bool ShouldTakeMemento(AggregateRoot aggregate);
        bool IsMementable(Type aggregateType);
    }
}
