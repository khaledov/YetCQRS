using System;

namespace YetCQRS.Domain.Exceptions
{
    public class ConcurrencyException : Exception
    {
        public ConcurrencyException(Guid id)
            : base(string.Format("A different version than expected was found in aggregate {0}", id))
        {
        }
    }
}
