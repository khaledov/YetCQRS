namespace YetCQRS.Events;

public class DefaultEventBus : IEventBus
{
 
  
    public async Task Publish<TEvent>(Guid streamId, params TEvent[] events) where TEvent : IEvent
    {
        throw new Exception();

        
    }
}
