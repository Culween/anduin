using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Anduin.EventBus.Events;

namespace Anduin.EventBus
{
    public class EventBus : IEventBus
    {
        private volatile bool _isConsuming = false;

        private readonly EventBusOptions _options;
        private readonly IMessageConsumer _messageConsumer;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly ILogger<EventBus> _logger;

        public EventBus(
            IOptions<EventBusOptions> options,
            IMessageConsumer messageConsumer,
            ILogger<EventBus> logger
            )
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            if (string.IsNullOrEmpty(_options.DefaultPublishTopic))
                throw new ArgumentNullException(nameof(_options.DefaultPublishTopic));

            _messageConsumer = messageConsumer ?? throw new ArgumentNullException(nameof(messageConsumer));
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
                        _messageConsumer.OnMessageReceived -= OnMessageReceived;
                        _messageConsumer.OnMessageReceived += OnMessageReceived;

                        var topics = _options.ConsumingTopics;
                        _messageConsumer.Subscribe(topics);
                        _messageConsumer.Listening(_cts.Token);
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

        public Task Publish(IntegrationEvent @event, string topic = null)
        {
            throw new NotImplementedException();
        }



        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            throw new NotImplementedException();
        }

        public void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            throw new NotImplementedException();
        }

        private void OnMessageReceived(object sender, MessageContext messageContext)
        {

        }
    }
}
