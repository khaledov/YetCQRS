using Optional;
using YetCQRS;
using YetCQRS.Commands;


public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    Task<Option<Error>> Execute(TCommand command, CancellationToken cancellationToken);
}

