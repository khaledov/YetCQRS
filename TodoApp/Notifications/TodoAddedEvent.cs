using YetCQRS.Events;

namespace TodoApp.Notifications;

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
