using Microsoft.Extensions.DependencyInjection;
using YetCQRS.Events;

namespace YetCQRS.Dispatchers;

/// <summary>
/// EventDispatcher is responsible for publishing events to their respective handlers.
/// </summary>
internal class EventDispatcher(IServiceProvider serviceLocator) : IEventDispatcher
{
    private readonly IServiceProvider _serviceLocator = serviceLocator ?? throw new ArgumentNullException(nameof(serviceLocator));

    /// <summary>
    /// Publishes an event to all registered handlers.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event.</typeparam>
    /// <param name="event">The event to publish.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no handler is found for the event.</exception>
    public Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken) where TEvent : class, IEvent
    {
        var handlers = _serviceLocator.GetServices<IEventHandler<TEvent>>() as List<IEventHandler<TEvent>>;
        if (handlers?.Count == 0)
            throw new InvalidOperationException($"Handler for {typeof(TEvent).Name} not found.");

        handlers?.ForEach(async handler =>
        {
            if (handler is not IEventHandler<TEvent> eventHandler)
                throw new InvalidOperationException($"Handler for {@event.GetType().Name} not found.");
            await handler.Handle(@event, cancellationToken);
        });
        return Task.CompletedTask;
    }
}
