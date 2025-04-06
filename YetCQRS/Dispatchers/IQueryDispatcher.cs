using YetCQRS.Queries;

namespace YetCQRS.Dispatchers;

public interface IQueryDispatcher
{
    Task<TResult> QueryAsync<TResult>(IQuery<TResult> query);
}
