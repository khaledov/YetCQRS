namespace YetCQRS.Queries;

public interface IQueryDispatcher
{
    Task<TResult> QueryAsync<TQuery,TResult>(TQuery query, CancellationToken cancellationToken) where TQuery:class, IQuery<TResult>;
}
