using JITDispatcher.Commands;
using JITDispatcher.Events;
using JITDispatcher.Queries;

namespace JITDispatcher.Dispatchers;

public class Dispatcher(ICommandDispatcher commandDispatcher,
    IQueryDispatcher queryDispatcher,
    IEventDispatcher eventDispatcher) : IDispatcher
{
    ICommandDispatcher CommandDispatcher { get; } = commandDispatcher;
    IQueryDispatcher QueryDispatcher { get; } = queryDispatcher;
    IEventDispatcher EventDispatcher { get; } = eventDispatcher;

    async Task IDispatcher.PublishAsync<T>(T @event, CancellationToken cancellationToken)
    {
        await EventDispatcher.PublishAsync(@event, cancellationToken);
    }

    Task<TResult> IDispatcher.QueryAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken)
    {
        return QueryDispatcher.QueryAsync<TQuery, TResult>(query, cancellationToken);
    }

    Task<ValidationResult> IDispatcher.SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken)
    {
        return CommandDispatcher.SendAsync<TCommand>(command, cancellationToken);
    }
}
