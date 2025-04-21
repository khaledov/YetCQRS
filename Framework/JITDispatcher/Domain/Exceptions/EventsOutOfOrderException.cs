using System;
using System.Runtime.Serialization;

namespace JITDispatcher.Domain.Exceptions
{
    [Serializable]
    internal class EventsOutOfOrderException : Exception
    {
        private Guid id;

        public EventsOutOfOrderException()
        {
        }

        public EventsOutOfOrderException(Guid id) :
            base(string.Format("Eventstore gave event for aggregate {0} out of order", id))
        {
            this.id = id;
        }

        public EventsOutOfOrderException(string message) : base(message)
        {
        }

        public EventsOutOfOrderException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EventsOutOfOrderException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}

