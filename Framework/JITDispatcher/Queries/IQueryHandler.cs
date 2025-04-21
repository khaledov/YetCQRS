

namespace JITDispatcher.Queries;

public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery<TResult>
{
    Task<TResult> Execute(TQuery query, CancellationToken cancellationToken);
}
