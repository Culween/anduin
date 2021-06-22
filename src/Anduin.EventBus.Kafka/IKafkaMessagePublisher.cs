using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Anduin.EventBus.Kafka
{
    public interface IKafkaMessagePublisher
    {
        string ServerAddress { get; }

        Task Publish(string topic, string routeKey, string messageTypeName, byte[] content);
    }
}
