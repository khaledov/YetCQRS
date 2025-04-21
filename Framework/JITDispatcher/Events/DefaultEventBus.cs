namespace JITDispatcher.Events;

public class DefaultEventBus : IEventBus
{
 
  
    public  Task Publish<TEvent>(Guid streamId, params TEvent[] events) where TEvent : IEvent
    {
        throw new NotImplementedException();

        
    }
}
