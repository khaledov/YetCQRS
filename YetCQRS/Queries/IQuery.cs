using MediatR;

namespace YetCQRS.Queries
{
    public interface IQuery<TResponse> : IRequest<TResponse>
    {
    }
}
