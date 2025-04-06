using Optional;
using YetCQRS.Commands;

namespace YetCQRS.Dispatchers;

public interface ICommandDispatcher<TCommand> where TCommand : ICommand
{
    Task<Option<TCommand, Error>> SendAsync(TCommand command) ;

}
