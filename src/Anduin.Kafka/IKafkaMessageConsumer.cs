using System;
using System.Collections.Generic;
using System.Threading;

namespace Anduin.Kafka
{
    public interface IKafkaMessageConsumer
    {
        string ServerAddress { get; }

        event EventHandler<MessageContext> OnMessageReceived;

        void Subscribe(IEnumerable<string> topics);

        void Listening(CancellationToken cancellationToken);

        void Commit();

        void Reject();
    }
}
