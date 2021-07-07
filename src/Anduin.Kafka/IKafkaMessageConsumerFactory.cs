using System.Collections.Generic;

namespace Anduin.Kafka
{
    public interface IKafkaMessageConsumerFactory
    {
        IKafkaMessageConsumer Create(IEnumerable<string> topics, string groupId, string connectionName);
    }
}
