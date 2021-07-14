using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;

namespace Anduin.Kafka
{
    public class ConsumerPool : IConsumerPool
    {
        protected KafkaOptions Options { get; }

        protected ConcurrentDictionary<string, IConsumer<string, byte[]>> Consumers { get; }

        public ILogger<ConsumerPool> Logger { get; set; }

        public ConsumerPool(
            IOptions<KafkaOptions> options,
            ILogger<ConsumerPool> logger)
        {
            Options = options.Value;
            Consumers = new ConcurrentDictionary<string, IConsumer<string, byte[]>>();
            Logger = logger;
        }

        public IConsumer<string, byte[]> Get(string groupId, string connectionName = null)
        {
            connectionName ??= KafkaConsumerConnections.DefaultConnectionName;

            return Consumers.GetOrAdd(
                connectionName, connection =>
                {
                    var config = new ConsumerConfig(Options.Consumers.GetOrDefault(connectionName))
                    {
                        GroupId = groupId,
                        EnableAutoCommit = false
                    };

                    Options.ConfigureConsumer?.Invoke(config);

                    return new ConsumerBuilder<string, byte[]>(config).Build();
                }
            );

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
