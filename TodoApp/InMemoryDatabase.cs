using TodoApp.Dtos;

namespace TodoApp;

internal static class InMemoryDatabase
{

    internal static readonly List<TodoDto> DataBase = new List<TodoDto>();
}