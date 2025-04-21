

namespace JITDispatcher.Queries;
public interface IQuery : IMessage
{
}
public interface IQuery<TResponse> : IQuery
{
}
