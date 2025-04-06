namespace YetCQRS.Events;

public interface IEvent : IMessage
{
    Guid Id { get; set; }
    int Version { get; set; }
    DateTimeOffset TimeStamp { get; set; }
}