using Newtonsoft.Json;
using SqlStreamStore;
using SqlStreamStore.Streams;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using YetCQRS.Events;
using YetCQRS.EventStore;

namespace YetCQRS.SqlStreamStore
{
    public class EventStore : IEventStore
    {
        public IEventBus EventBus { get; set; }
        IStreamStore _streamStore;
        public EventStore(IEventBus eventBus,
            IStreamStore streamStore)
        {
            EventBus = eventBus;
            _streamStore = streamStore;
        }
        public  IEnumerable LoadEventsFor(Guid aggregateId, int fromVersion)
        {
            var endOfStream = false;
            var startVersion = fromVersion;
            while(!endOfStream)
            {
                var stream =  _streamStore.ReadStreamForwards(aggregateId.ToString(), startVersion, 10).GetAwaiter().GetResult();
                endOfStream = stream.IsEnd;
                startVersion = stream.NextStreamVersion;
                foreach (var msg in stream.Messages)
                    yield return  JsonConvert.DeserializeObject( msg.GetJsonData().GetAwaiter().GetResult(), type: Type.GetType(msg.Type));
               
            }

           
        }

        public void Save(Guid aggregateId, Event @event)
        {

            var msg = new NewStreamMessage(aggregateId, @event.GetType().ToString(), JsonConvert.SerializeObject(@event));
           
            _streamStore.AppendToStream(aggregateId.ToString(), ExpectedVersion.Any, msg);
            EventBus.Publish(aggregateId, @event);
        }
    }
}
