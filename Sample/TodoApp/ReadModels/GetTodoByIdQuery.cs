using TodoApp.Dtos;
using JITDispatcher.Queries;

namespace TodoApp.ReadModels;

internal class GetTodoByIdQuery : IQuery<TodoDto>
{
    public GetTodoByIdQuery(Guid id)
    {
        Id = id;
    }
    public Guid Id { get; }
}
