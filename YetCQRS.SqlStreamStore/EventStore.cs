﻿using Newtonsoft.Json;
using SqlStreamStore;
using SqlStreamStore.Streams;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        public void Save(Guid aggregateId, IList<Event> newEvents)
        {

            if (newEvents.Count == 0)
                return;
            
            foreach (var e in newEvents)
                if (e.Id != aggregateId)
                    throw new InvalidOperationException(
                        "Cannot save events reporting inconsistent aggregate IDs");
            var eventsLoaded = newEvents.Count;
            var expected = eventsLoaded == 0 ? ExpectedVersion.NoStream : eventsLoaded - 1;
           
            _streamStore.AppendToStream(aggregateId.ToString(), expected, newEvents
                .Cast<dynamic>()
                .Select(e => new NewStreamMessage(e.Id, e.GetType().ToString(), JsonConvert.SerializeObject(e))).ToArray());

            EventBus.Publish(aggregateId, newEvents.ToArray());
        }
    }
}
