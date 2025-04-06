using Optional;
using YetCQRS.Commands;

namespace YetCQRS.Dispatchers;

public class CommandDispatcher<TCommand> : ICommandDispatcher<TCommand> where TCommand : ICommand
{
    private readonly IServiceProvider _serviceLocator;

    /// <summary>
    /// Initialize a new instance of <cref>CommandDispatcher</cref> class.
    /// </summary>
    /// <param name="serviceLocator">Service locator that can resolve all handlers</param>
    public CommandDispatcher(IServiceProvider serviceLocator)
    {
        _serviceLocator = serviceLocator ?? throw new ArgumentNullException(nameof(serviceLocator));
    }

    public async Task<Option<TCommand, Error>> SendAsync(TCommand command)
    {
        if (command == null) throw new ArgumentNullException(nameof(command));

        var handler = _serviceLocator.GetService(typeof(BaseCommandHandler<TCommand>)) as BaseCommandHandler<TCommand>;
        if (handler == null) throw new InvalidOperationException($"Handler for {typeof(TCommand).Name} not found.");

        return await handler.Handle(command, CancellationToken.None);
    }
}
