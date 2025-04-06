using TodoApp.Dtos;
using TodoApp.WriteModels;

namespace TodoApp.Handlers;

internal class AddTodoHandler : ICommandHandler<AddTodoCommand>
{


    public Task Execute(AddTodoCommand command, CancellationToken cancellationToken)
    {
        var todo = new TodoDto(command.Id, command.Title, command.Description, command.IsCompleted);

        InMemoryDatabase.DataBase.Add(todo);
        return Task.CompletedTask;
    }
}
