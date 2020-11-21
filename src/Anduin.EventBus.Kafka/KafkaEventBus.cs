using System;
using System.Threading;
using Microsoft.Extensions.Logging;
using Anduin.EventBus.Kafka.Serializers;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Anduin.EventBus.Kafka
{
    public class KafkaEventBus : EventBusBase, IEventBus
    {
        private volatile bool _isConsuming = false;

        private readonly EventBusOptions _options;
        private readonly IMessagePublisher _publisher;
        private readonly IMessageConsumer _consumer;
        private readonly IEventSerializer _serializer;
        private readonly ILogger<KafkaEventBus> _logger;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        public KafkaEventBus(
            IOptions<KafkaEventBusOptions> options,
            IMessagePublisher publisher,
            IMessageConsumer consumer,
            IEventSerializer serializer,
            ILogger<KafkaEventBus> logger
            ) : base()
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            if (string.IsNullOrEmpty(_options.DefaultPublishTopic))
                throw new ArgumentNullException(nameof(_options.DefaultPublishTopic));

            _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
            _consumer = consumer ?? throw new ArgumentNullException(nameof(consumer));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            _logger = logger;
        }

        public void Start()
        {
            if (!_isConsuming)
            {
                _isConsuming = true;
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        _consumer.OnMessageReceived -= OnMessageReceived;
                        _consumer.OnMessageReceived += OnMessageReceived;

                        var topics = _options.ConsumingTopics;
                        _consumer.Subscribe(topics);
                        _consumer.Listening(_cts.Token);
                    }
                    catch (OperationCanceledException)
                    {
                        // Ignore..
                    }
                }, _cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default)
                .ContinueWith(t =>
                {
                    _isConsuming = false;
                    if (t.IsFaulted)
                    {
                        _logger.LogError(t.Exception.Flatten(), "Message consumer listening error.");
                        // Restart
                        Start();
                    }
                });
            }
        }

    }
}
