using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Anduin.EventBus.Events;
using System.Collections.Generic;
using System.Reflection;
using System.Collections.Concurrent;

namespace Anduin.EventBus
{
    public abstract class EventBusBase : IEventBus
    {
        private volatile bool _isConsuming = false;

        private readonly IMessagePublisher _publisher;
        private readonly IMessageConsumer _consumer;
        private readonly ILogger<EventBusBase> _logger;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        /// <summary>
        /// 定义线程安全集合
        /// </summary>
        private readonly ConcurrentDictionary<Type, List<Type>> _eventAndHandlerMapping;

        public EventBusBase(
            IMessagePublisher publisher,
            IMessageConsumer consumer,
            ILogger<EventBusBase> logger
            )
        {
            _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
            _consumer = consumer ?? throw new ArgumentNullException(nameof(consumer));
            _logger = logger;
            _eventAndHandlerMapping = new ConcurrentDictionary<Type, List<Type>>();
        }


        private void OnMessageReceived(object sender, MessageContext messageContext)
        {

        }

        private string GetRouteKey(IntegrationEvent @event)
        {
            return @event.RouteKey ?? @event.Id;
        }

        public Task PublishAsync<TEventData>(TEventData eventData) where TEventData : IEvent
        {
            return PublishAsync(typeof(TEventData), eventData);
        }

        public abstract Task PublishAsync(Type eventType, object eventData);

        protected virtual async Task TriggerHandlerAsync(Type eventType, object eventData, List<Exception> exceptions)
        {

        }

        protected virtual void SubscribeHandlers()
        {
            Assembly assembly = Assembly.GetEntryAssembly();
            foreach (var type in assembly.GetTypes())
            {
                if (typeof(IEventHandler<IEvent>).IsAssignableFrom(type))
                {
                    Type handlerInterface = type.GetInterface("IEventHandler`1");
                    if (handlerInterface != null)
                    {
                        Type eventType = handlerInterface.GetGenericArguments()[0];

                        if (_eventAndHandlerMapping.ContainsKey(eventType))
                        {
                            List<Type> handlerTypes = _eventAndHandlerMapping[eventType];
                            handlerTypes.Add(type);
                            _eventAndHandlerMapping[eventType] = handlerTypes;
                        }
                        else
                        {
                            var handlerTypes = new List<Type> { type };
                            _eventAndHandlerMapping[eventType] = handlerTypes;
                        }
                    }
                }
            }
        }

        public void Subscribe<TEvent, THandler>()
            where TEvent : IEvent
            where THandler : IEventHandler<TEvent>
        {
            throw new NotImplementedException();
        }

        public void Unsubscribe<TEvent, THandler>()
            where TEvent : IEvent
            where THandler : IEventHandler<TEvent>
        {
            throw new NotImplementedException();

        }

        protected class EventTypeWithEventHandlerFactories
        {
            public Type EventType { get; }
        }
    }
}
