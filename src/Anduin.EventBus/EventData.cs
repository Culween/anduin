using System;

namespace Anduin.EventBus
{
    public class EventData : IEventData
    {
        public DateTime EventTime { get; set; }

        public object EventSource { get; set; }

        public EventData()
        {
            EventTime = DateTime.Now;
        }
    }
}
