namespace YetCQRS.Commands;

public interface ICommandDispatcher
{
    Task<ValidationResult> SendAsync<TCommand>(TCommand command,CancellationToken cancellationToken) where TCommand:class, ICommand;

}
