using YetCQRS.Queries;

namespace YetCQRS.Dispatchers;

public class QueryDispatcher : IQueryDispatcher
{
    private readonly IServiceProvider _serviceLocator;

    /// <summary>
    /// Initialize a new instance of <cref>QueryDispatcher</cref> class.
    /// </summary>
    /// <param name="serviceLocator">Service locator that can resolve all handlers</param>
    public QueryDispatcher(IServiceProvider serviceLocator)
    {
        _serviceLocator = serviceLocator ?? throw new ArgumentNullException(nameof(serviceLocator));
    }
    public async Task<TResult> QueryAsync<TResult>(IQuery<TResult> query)
    {
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var handler = _serviceLocator.GetService(typeof(IQueryHandler<IQuery<TResult>, TResult>)) as IQueryHandler<IQuery<TResult>, TResult>;
            if (handler == null) throw new InvalidOperationException($"Handler for {typeof(IQuery<TResult>).Name} not found.");

            return await handler.Execute(query, CancellationToken.None);
        }
    }

}