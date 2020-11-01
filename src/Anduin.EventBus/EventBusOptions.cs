using System;
using System.Collections.Generic;
using System.Text;

namespace Anduin.EventBus
{
    public class EventBusOptions
    {
        public string DefaultPublishTopic { get; set; }

        public List<string> ConsumingTopics { get; set; }
    }
}
