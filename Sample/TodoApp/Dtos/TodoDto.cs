namespace TodoApp.Dtos;

internal record TodoDto(Guid Id, string Title, string Description, bool IsCompleted);
