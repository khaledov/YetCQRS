// See https://aka.ms/new-console-template for more information


using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TodoApp.Dtos;
using TodoApp.Notifications;
using TodoApp.ReadModels;
using TodoApp.WriteModels;
using JITDispatcher.DependencyInjection;
using JITDispatcher.Dispatchers;

var services = new ServiceCollection();
services.AddJITDispatcher(Assembly.GetExecutingAssembly());
 
var serviceProvider = services.BuildServiceProvider();

Console.WriteLine("Enter Your Todo Title!");
string title = Console.ReadLine();
var cmd =new AddTodoCommand(title: title);
var dispatcher = serviceProvider.GetRequiredService<IDispatcher>();
var result=dispatcher.SendAsync(cmd, CancellationToken.None).GetAwaiter().GetResult(); 
if(result.IsValid)
{
    var @event=new TodoAddedEvent(cmd.Id,cmd.Title);
   await dispatcher.PublishAsync(@event, CancellationToken.None);
}else
    Console.WriteLine(result.ErrorMessages.ToList().Select(e=>e));
Console.WriteLine("*************************************");

Console.WriteLine("Enter Todo Id to get it from DB");
string id = Console.ReadLine();
var query = new GetTodoByIdQuery(Guid.Parse(id));
var todo = await dispatcher.QueryAsync<GetTodoByIdQuery, TodoDto>(query, CancellationToken.None);
Console.WriteLine($"Todo Title: {todo.Title}");
Console.ReadLine();