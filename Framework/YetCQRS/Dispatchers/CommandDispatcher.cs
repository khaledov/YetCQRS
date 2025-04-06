using YetCQRS.Commands;
using ValidationResult = YetCQRS.Commands.ValidationResult;

namespace YetCQRS.Dispatchers;

/// <summary>
/// Initialize a new instance of <cref>CommandDispatcher</cref> class.
/// </summary>
/// <param name="serviceLocator">Service locator that can resolve all handlers</param>
internal class CommandDispatcher(IServiceProvider serviceLocator) : ICommandDispatcher
{
    private readonly IServiceProvider _serviceLocator = serviceLocator ?? throw new ArgumentNullException(nameof(serviceLocator));

    /// <summary>
    /// Sends the specified command asynchronously.
    /// </summary>
    /// <param name="command">The command to send.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the validation result.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the command is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the handler or validator for the command is not found.</exception>
    public async Task<ValidationResult> SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken)where TCommand:class,ICommand
    {
        ArgumentNullException.ThrowIfNull(command);

        var handler = _serviceLocator.GetService(typeof(ICommandHandler<TCommand>)) as ICommandHandler<TCommand>??
            throw new InvalidOperationException($"Handler for {typeof(TCommand).Name} not found.");

        var validator = _serviceLocator.GetService(typeof(ICommandValidator<TCommand>)) as ICommandValidator<TCommand>??
            throw new InvalidOperationException("It seems that you tried to instantiate a command handler without a validator.");

        var result = validator.Validate(command);

        if (result.IsValid)
            await handler.Execute(command, cancellationToken);

        return result;
    }

  
}
