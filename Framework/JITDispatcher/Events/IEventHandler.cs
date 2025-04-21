namespace JITDispatcher.Events;

public interface IEventHandler<TEvent> where TEvent :class, IEvent
{
    Task Handle(TEvent @event, CancellationToken cancellationToken);
}
