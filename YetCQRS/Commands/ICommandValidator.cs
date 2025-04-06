namespace YetCQRS.Commands;

public interface ICommandValidator
{
    ValidationResult Validate(ICommand command);
}
public interface ICommandValidator<TCommand> : ICommandValidator where TCommand : ICommand
{

}
