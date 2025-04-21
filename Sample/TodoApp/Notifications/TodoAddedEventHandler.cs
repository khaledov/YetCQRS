using JITDispatcher.Events;

namespace TodoApp.Notifications;

internal class TodoAddedEventHandler : IEventHandler<TodoAddedEvent>
{
    public async Task Handle(TodoAddedEvent @event, CancellationToken cancellationToken)
    {
        await Task.Run(() =>
        {
            Console.WriteLine($"Todo added: {@event.Id} - {@event.Title}");
        }, cancellationToken);
    }
}
