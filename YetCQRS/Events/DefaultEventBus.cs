using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace YetCQRS.Events
{
    public class DefaultEventBus : IEventBus
    {
        readonly IMediator _mediator;
        public DefaultEventBus(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<Unit> Publish<TEvent>(Guid streamId, params TEvent[] events) where TEvent : Event
        {
            await Task.Run(() => events.ToList().ForEach(async e => await _mediator.Publish(e)));

            return Unit.Value;
        }
    }
}
