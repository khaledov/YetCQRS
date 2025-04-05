using Optional;
using Optional.Async;

namespace YetCQRS.Commands;


    /// <summary>
    /// Base class for command handlers.
    /// </summary>
    /// <typeparam name="TCommand">The type of the command.</typeparam>
    public abstract class BaseCommandHandler<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
    {
        /// <summary>
        /// Gets the validator for the command.
        /// </summary>
        protected ICommandValidator<TCommand> Validator { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCommandHandler{TCommand}"/> class.
        /// </summary>
        /// <param name="validator">The validator for the command.</param>
        /// <exception cref="InvalidOperationException">Thrown when the validator is null.</exception>
        public BaseCommandHandler(ICommandValidator<TCommand> validator)
        {
            Validator = validator ?? throw new InvalidOperationException("It seems that you tried to instantiate a command handler without a validator.");
        }

    /// <summary>
    /// Handles the command.
    /// </summary>
    /// <param name="request">The command to handle.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An option containing an error if the handling fails.</returns>
    public Task<Option<Error>> Handle(TCommand command, CancellationToken cancellationToken) =>
          ValidateCommand(command).FlatMapAsync(cmd => Execute(cmd, cancellationToken));





    /// <summary>
    /// Validates the command.
    /// </summary>
    /// <param name="command">The command to validate.</param>
    /// <returns>An option containing the command if it is valid, or an error if it is not.</returns>
    protected Option<TCommand, Error> ValidateCommand(TCommand command)
        {
            var validationResult = Validator.Validate(command);

            return validationResult
                .SomeWhen(
                    r => r.IsValid,
                    r => Error.Validation(r.ErrorMessages.Select(e => e)))
                .Map(_ => command);
        }

    public abstract Task<Option<Error>> Execute(TCommand command, CancellationToken cancellationToken);
}
