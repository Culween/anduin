using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Anduin.EventBus
{
    public interface IMessagePublisher : IDisposable
    {
        string ServerAddress { get; }

        Task Publish(string topic, string routeKey, string messageTypeName, byte[] content);
    }
}
