// See https://aka.ms/new-console-template for more information


using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TodoApp.Notifications;
using TodoApp.WriteModels;
using YetCQRS.DependencyInjection;
using YetCQRS.Dispatchers;

var services = new ServiceCollection();
services.AddYetCQRS(Assembly.GetExecutingAssembly());
 
var serviceProvider = services.BuildServiceProvider();

Console.WriteLine("Enter Your Todo Title!");
string title = Console.ReadLine();
var cmd =new AddTodoCommand(title: title);
var dispatcher = serviceProvider.GetRequiredService<IDispatcher>();
var result=dispatcher.SendAsync(cmd, CancellationToken.None).GetAwaiter().GetResult(); 
if(result.IsValid)
{
    var @event=new TodoAddedEvent(cmd.Title);
   await dispatcher.PublishAsync(@event, CancellationToken.None);
}else
    Console.WriteLine(result.ErrorMessages.ToList().Select(e=>e));
Console.ReadLine();