using System.ComponentModel.DataAnnotations;

namespace YetCQRS.Commands;

public interface ICommandValidator
{
    ValidationResult Validate(object command);
}
public interface ICommandValidator<TCommand> : ICommandValidator where TCommand : ICommand
{

}
