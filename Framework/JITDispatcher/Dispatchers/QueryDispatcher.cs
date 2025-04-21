using JITDispatcher.Queries;

namespace JITDispatcher.Dispatchers;

/// <summary>
/// Initialize a new instance of <cref>QueryDispatcher</cref> class.
/// </summary>
/// <param name="serviceLocator">Service locator that can resolve all handlers</param>
internal class QueryDispatcher(IServiceProvider serviceLocator) : IQueryDispatcher
{
    private readonly IServiceProvider _serviceLocator = serviceLocator ?? throw new ArgumentNullException(nameof(serviceLocator));

    /// <summary>
    /// Asynchronously executes the specified query and returns the result.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the query.</typeparam>
    /// <param name="query">The query to execute.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the result of the query.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the query is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the handler for the query is not found.</exception>
    public async Task<TResult> QueryAsync<TQuery,TResult>(TQuery query, CancellationToken cancellationToken)where TQuery:class, IQuery<TResult> 
    {
        ArgumentNullException.ThrowIfNull(query);

        var handler = _serviceLocator.GetService(typeof(IQueryHandler<TQuery, TResult>)) as IQueryHandler<TQuery, TResult> 
            ?? throw new InvalidOperationException($"Handler for {typeof(TQuery).Name} not found.");

        return await handler.Execute(query, cancellationToken);
    }

}