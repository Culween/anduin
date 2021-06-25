using Confluent.Kafka;
using System.Collections.Generic;

namespace Anduin.EventBus.Kafka
{
    public class KafkaEventBusOptions
    {
        public string DefaultPublishTopic { get; set; }
        
        public string GroupId { get; set; }

        public ProducerConfig Producer { get; set; }

        public ConsumerConfig Consumer { get; set; }

        public List<string> TopicsForConsume { get; set; }
    }
}
