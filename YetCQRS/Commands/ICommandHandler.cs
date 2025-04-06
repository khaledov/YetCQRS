using Optional;
using YetCQRS;
using YetCQRS.Commands;


public interface ICommandHandler< TCommand> where TCommand : ICommand
{
    Task<Option<TCommand, Error>> Execute(TCommand command, CancellationToken cancellationToken);
}

