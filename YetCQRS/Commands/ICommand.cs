using MediatR;
using Optional;

namespace YetCQRS.Commands
{
    public interface ICommand :
        IRequest<Option<Unit, Error>>
    {
    }

    public interface ICommand<TResult> :
        IRequest<Option<TResult, Error>>
    {
    }
}
