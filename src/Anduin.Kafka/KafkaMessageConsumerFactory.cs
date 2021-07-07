using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Anduin.Kafka
{
    public class KafkaMessageConsumerFactory : IKafkaMessageConsumerFactory, IDisposable
    {
        protected IServiceScope ServiceScope { get; }

        public KafkaMessageConsumerFactory(IServiceScopeFactory serviceScopeFactory)
        {
            ServiceScope = serviceScopeFactory.CreateScope();
        }

        public IKafkaMessageConsumer Create(IEnumerable<string> topics, string groupId, string connectionName)
        {
            var consumer = ServiceScope.ServiceProvider.GetRequiredService<KafkaMessageConsumer>();
            consumer.Initialize(topics, groupId, connectionName);
            return consumer;
        }

        public void Dispose()
        {
            ServiceScope.Dispose();
        }
    }
}
