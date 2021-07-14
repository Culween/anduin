using System;
using Confluent.Kafka;

namespace Anduin.Kafka
{
    public interface IConsumerPool : IDisposable
    {
        IConsumer<string, byte[]> Get(string groupId, string connectionName = null);
    }
}
