[![JITDispatcher](https://github.com/khaledov/JITDispatcher/blob/master/assets/JitDispatcher-132.png)
# JITDispatcher

**Tiny framework for whom wants to taste CQRS**

![License](https://img.shields.io/github/license/khaledov/JITDispatcher)
![Language](https://img.shields.io/github/languages/top/khaledov/JITDispatcher)

## Table of Contents

1. [Introduction](#introduction)
2. [Features](#features)
3. [Getting Started](#getting-started)
4. [Installation](#installation)
5. [Usage](#usage)
6. [Contributing](#contributing)
7. [License](#license)

---

## Introduction

JITDispatcher is a lightweight framework for developers who are interested in exploring the **Command Query Responsibility Segregation (CQRS)** pattern. It provides a simple and efficient way to implement CQRS in your applications, making it ideal for learning or small projects.

---

## Features

- **Lightweight and Simple**: Easy to integrate into your projects.
- **CQRS Pattern**: Implements the separation of commands and queries for better scalability and maintainability.
- **C# Language**: Built entirely in C# for .NET developers.

---

## Getting Started

These instructions will help you get started with JITDispatcher in your project.

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) installed on your machine.
- Basic knowledge of C# and CQRS.

---

## Installation

To install JITDispatcher, you can use the NuGet Package Manager.

```bash
dotnet add package JITDispatcher
```

Alternatively, you can clone the repository and include it in your project manually:

```bash
git clone https://github.com/khaledov/JITDispatcher.git
```

---

## Usage

Below is an example of how to use JITDispatcher in your project:

1. **Define Commands and its Validator**:
   ```csharp
   internal class AddTodoCommand : ICommand
    {
        public AddTodoCommand(string title)
        {
           ........
        }
        .............
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


   ```
2. **Define Queries**
```csharp
    internal class GetTodoByIdQuery : IQuery<TodoDto>
    {
        public GetTodoByIdQuery(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; }
    }

```
3. **Define Events**
```csharp
    internal class TodoAddedEvent : IEvent
    {
        public readonly string Title;
        public TodoAddedEvent(Guid id,string title)
        {
            Id = id;
            Title = title;
        }
        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
    }


```
4. **Create CommandHandlers**:
   ```csharp
    internal class AddTodoHandler : ICommandHandler<AddTodoCommand>
    {
    
    
        public Task Execute(AddTodoCommand command, CancellationToken cancellationToken)
        {
            var todo = new TodoDto(command.Id, command.Title, command.Description, command.IsCompleted);
    
            InMemoryDatabase.DataBase.Add(todo);
            return Task.CompletedTask;
        }
    }
    
    ```
5. **Create Query Handler**
   ```csharp
    internal class GetTodoByIdQueryHandler : IQueryHandler<GetTodoByIdQuery, TodoDto>
    {
    
        public async Task<TodoDto> Execute(GetTodoByIdQuery query, CancellationToken cancellationToken)
        {
            var todo = InMemoryDatabase.DataBase.FirstOrDefault(t => t.Id == query.Id);
            return await Task.FromResult(todo);
        }
    }
     ```
6.  **Create Event Handler**

    ```csharp
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

    
      ```

7. **Register to Service collection in startup.cs**:
   
   ```csharp
   services.AddJITDispatcher(Assembly.GetExecutingAssembly());
   ```
8. **Resolve Dispatcher wherever you want to invoke it**
  ```csharp   
      MyConstructor(IDispatcher dispatcher)
      {
          _dispatcher=dispatcher;
      }
      
      ....
      ....
      ....
      //Command
      var cmd =new AddTodoCommand(title: title);
      var result=dispatcher.SendAsync(cmd, CancellationToken.None).GetAwaiter().GetResult(); 
        if(result.IsValid)
        {
            //Events
            var @event=new TodoAddedEvent(cmd.Id,cmd.Title);
           await dispatcher.PublishAsync(@event, CancellationToken.None);
        }else
            Console.WriteLine(result.ErrorMessages.ToList().Select(e=>e));
        Console.WriteLine("*************************************");
       
       //Query 
      var query = new GetTodoByIdQuery(id);
      var todo = await dispatcher.QueryAsync<GetTodoByIdQuery, TodoDto>(query, CancellationToken.None);
     
   ```

---

## Contributing

Contributions are welcome! If you would like to contribute to this project, please follow these steps:

1. Fork the repository.
2. Create a new branch (`git checkout -b feature/YourFeature`).
3. Commit your changes (`git commit -m 'Add YourFeature'`).
4. Push to the branch (`git push origin feature/YourFeature`).
5. Open a pull request.

---

## License

This project is licensed under the [Apache2.0](LICENSE).

---

## Acknowledgments

- Inspired by the principles of CQRS.
- Special thanks to the open-source community for their contributions.

---

Feel free to reach out if you have any questions or suggestions!
