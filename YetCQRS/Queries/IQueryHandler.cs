
namespace YetCQRS.Queries;

public interface IQueryHandler<in TQuery, TResponse> where TQuery : IQuery<TResponse>
{
}
