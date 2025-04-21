
using JITDispatcher.Commands;

namespace TodoApp.WriteModels;

internal class AddTodoCommand : ICommand
{
    public AddTodoCommand(string title)
    {
        Id = Guid.NewGuid();
        Title = title;
    }
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public bool IsCompleted { get; set; }
}
internal class AddTodoCommandValidator : ICommandValidator<AddTodoCommand>
{
   
    

    public ValidationResult Validate(AddTodoCommand command)
    {
        var result = new ValidationResult();
        if (string.IsNullOrWhiteSpace(command.Title))
        {
            result.ErrorMessages.Add("Title is required");
        }
       
        return result;
    }


}
