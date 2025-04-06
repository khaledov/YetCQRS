﻿using YetCQRS.Commands;
using YetCQRS.Events;
using YetCQRS.Queries;

namespace YetCQRS.Dispatchers;

public interface IDispatcher
{
    Task<ValidationResult> SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken) where TCommand : class, ICommand;
    Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : class, IEvent;
    Task<TResult> QueryAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken) where TQuery : class, IQuery<TResult>;
}