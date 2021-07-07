using System;
using System.Threading.Tasks;

namespace Anduin.Kafka
{
    public interface IMessagePublisher : IDisposable
    {
        string ServerAddress { get; }

        Task Publish(string topic, string routeKey, string messageTypeName, byte[] content);
    }
}
