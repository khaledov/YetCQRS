using TodoApp.Dtos;
using TodoApp.ReadModels;
using JITDispatcher.Queries;

namespace TodoApp.Handlers;

internal class GetTodoByIdQueryHandler : IQueryHandler<GetTodoByIdQuery, TodoDto>
{

    public async Task<TodoDto> Execute(GetTodoByIdQuery query, CancellationToken cancellationToken)
    {
        var todo = InMemoryDatabase.DataBase.FirstOrDefault(t => t.Id == query.Id);
        return await Task.FromResult(todo);
    }
}
