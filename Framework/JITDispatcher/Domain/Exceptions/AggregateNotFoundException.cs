using System;

namespace JITDispatcher.Domain.Exceptions
{
    public class AggregateNotFoundException : System.Exception
    {
        public AggregateNotFoundException(Guid id)
            : base(string.Format("Aggregate {0} was not found", id))
        {
        }
    }
}
