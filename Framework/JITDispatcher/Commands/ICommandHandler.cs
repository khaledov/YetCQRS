using JITDispatcher.Commands;


public interface ICommandHandler<TCommand> where TCommand : ICommand
{
    Task Execute(TCommand command, CancellationToken cancellationToken);
}

