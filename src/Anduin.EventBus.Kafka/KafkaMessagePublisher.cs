using System;
using System.Threading.Tasks;

namespace Anduin.EventBus.Kafka
{
    public class KafkaMessagePublisher : IMessagePublisher
    {
        public string ServerAddress => throw new NotImplementedException();

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task Publish(string topic, string routeKey, string messageTypeName, byte[] content)
        {
            throw new NotImplementedException();
        }
    }
}
