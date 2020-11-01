using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Anduin.EventBus
{
    public interface IMessageConsumer
    {
        string ServerAddress { get; }

        event EventHandler<MessageContext> OnMessageReceived;

        void Subscribe(IEnumerable<string> topics);

        void Listening(CancellationToken cancellationToken);

        void Commit();

        void Reject();
    }
}
