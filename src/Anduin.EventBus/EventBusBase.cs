using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Anduin.EventBus.Events;
using System.Collections.Generic;

namespace Anduin.EventBus
{
    public abstract class EventBusBase : IEventBus
    {
        private volatile bool _isConsuming = false;

        private readonly IMessagePublisher _publisher;
        private readonly IMessageConsumer _consumer;
        private readonly ILogger<EventBusBase> _logger;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        public EventBusBase(
            IMessagePublisher publisher,
            IMessageConsumer consumer,
            ILogger<EventBusBase> logger
            )
        {
            _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
            _consumer = consumer ?? throw new ArgumentNullException(nameof(consumer));
            _logger = logger;
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

        protected class EventTypeWithEventHandlerFactories
        {
            public Type EventType { get; }
        }
    }
}
