using MediatR;
using System;
using System.Threading.Tasks;

namespace YetCQRS.Events
{
    public interface IEventBus
    {
        Task<Unit> Publish<TEvent>(Guid streamId, params TEvent[] events) where TEvent : Event;
    }
}
