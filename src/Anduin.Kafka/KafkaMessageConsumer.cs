using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Anduin.Kafka
{
    public class KafkaMessageConsumer : IKafkaMessageConsumer
    {
        public string ServerAddress => throw new NotImplementedException();

        public event EventHandler<MessageContext> OnMessageReceived;

        protected string ConnectionName { get; private set; }

        protected string GroupId { get; private set; }

        protected IEnumerable<string> Topics { get; private set; }

        public virtual void Initialize(
            IEnumerable<string> topics,
            string groupId,
            string connectionName = null)
        {
            Topics = Topics ?? throw new ArgumentNullException(nameof(topics));
            GroupId = groupId ?? throw new ArgumentNullException(nameof(groupId));
            ConnectionName = connectionName ?? KafkaConsumerConnections.DefaultConnectionName;
        }

        public void Commit()
        {
            throw new NotImplementedException();
        }

        public void Listening(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Reject()
        {
            throw new NotImplementedException();
        }

        public void Subscribe(IEnumerable<string> topics)
        {
            throw new NotImplementedException();
        }
    }
}
