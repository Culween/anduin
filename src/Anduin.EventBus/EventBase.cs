using System;

namespace Anduin.EventBus
{
    public class EventBase : IEvent
    {
        public DateTime EventTime { get; set; }

        public object EventSource { get; set; }

        public EventBase()
        {
            EventTime = DateTime.Now;
        }
    }
}
