using Confluent.Kafka;
using Confluent.Kafka.Admin;
using System;

namespace Anduin.Kafka
{
    public class KafkaOptions
    {
        public KafkaProducerConnections Producers { get; }
        
        public KafkaConsumerConnections Consumers { get; }

        public Action<ProducerConfig> ConfigureProducer { get; set; }

        public Action<ConsumerConfig> ConfigureConsumer { get; set; }

        public Action<TopicSpecification> ConfigureTopic { get; set; }

        public KafkaOptions()
        {
            Producers = new KafkaProducerConnections();
            Consumers = new KafkaConsumerConnections();
        }
    }
}
