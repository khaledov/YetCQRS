using YetCQRS.Events;

namespace TodoApp.Notifications;

internal class TodoAddedEvent : IEvent
{
    public readonly string Title;
    public TodoAddedEvent(string title)
    {
        Id = Guid.NewGuid();
        Title = title;
    }
    public Guid Id { get; set; }
    public int Version { get; set; }
    public DateTimeOffset TimeStamp { get; set; }
}
