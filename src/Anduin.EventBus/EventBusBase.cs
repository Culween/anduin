using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Anduin.EventBus.Events;
using Anduin.EventBus.Serializers;
using System.Collections.Generic;

namespace Anduin.EventBus
{
    public abstract class EventBusBase : IEventBus
    {
        private volatile bool _isConsuming = false;

        private readonly EventBusOptions _options;
        private readonly IMessagePublisher _publisher;
        private readonly IMessageConsumer _consumer;
        private readonly IEventSerializer _serializer;
        private readonly ILogger<EventBusBase> _logger;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        public EventBusBase(
            IOptions<EventBusOptions> options,
            IMessagePublisher publisher,
            IMessageConsumer consumer,
            IEventSerializer serializer,
            ILogger<EventBusBase> logger
            )
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

        public async Task PublishAsync(IntegrationEvent @event)
        {
            //topic = topic ?? _options.DefaultPublishTopic;
            try
            {
                byte[] eventBytes = _serializer.Serialize(@event);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IEventHandler<T>
        {
            throw new NotImplementedException();
        }

        public void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IEventHandler<T>
        {
            throw new NotImplementedException();
        }

        private void OnMessageReceived(object sender, MessageContext messageContext)
        {

        }

        private string GetRouteKey(IntegrationEvent @event)
        {
            return @event.RouteKey ?? @event.Id;
        }

        public Task PublishAsync<TEventData>(TEventData eventData) where TEventData : IEventData
        {
            return PublishAsync(typeof(TEventData), eventData);
        }

        public abstract Task PublishAsync(Type eventType, object eventData);

        protected virtual async Task TriggerHandlerAsync(Type eventType, object eventData, List<Exception> exceptions)
        {

        }
    }
}
