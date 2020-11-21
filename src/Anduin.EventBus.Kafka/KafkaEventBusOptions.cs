using System.Collections.Generic;

namespace Anduin.EventBus.Kafka
{
    public class KafkaEventBusOptions
    {
        public string DefaultPublishTopic { get; set; }

        public List<string> ConsumingTopics { get; set; }
    }
}
