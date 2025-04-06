namespace YetCQRS.Commands;


public interface ICommandValidator<TCommand>  where TCommand : ICommand
{
    ValidationResult Validate(TCommand command);
}
