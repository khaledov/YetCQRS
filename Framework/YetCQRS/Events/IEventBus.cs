namespace YetCQRS.Events
{
    public interface IEventBus
    {
        Task Publish<TEvent>(Guid streamId, params TEvent[] events) where TEvent : IEvent;
    }
}
