using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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
        protected ConcurrentDictionary<Type, List<Type>> EventAndHandlerMapping { get; }

        public EventBusBase(
            //IMessagePublisher publisher,
            //IMessageConsumer consumer,
            //ILogger<EventBusBase> logger
            )
        {
            //_publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
            //_consumer = consumer ?? throw new ArgumentNullException(nameof(consumer));
            //_logger = logger;
            EventAndHandlerMapping = new ConcurrentDictionary<Type, List<Type>>();
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

        /// <summary>
        /// 本地或分布式 EventBus 各自实现该方法。
        /// 本地实现 PublishAsync 调用 TriggerHandlersAsync，
        /// 分布式实现 PublishAsync 采用分布式消息队列发送消息，另外实现订阅消息调用 TriggerHandlersAsync。
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="eventData"></param>
        /// <returns></returns>
        public abstract Task PublishAsync(Type eventType, object eventData);

        protected virtual async Task TriggerHandlersAsync(Type eventType, object eventData, List<Exception> exceptions)
        {
            // 支持多个 Handler 订阅同一个事件
            if (EventAndHandlerMapping.ContainsKey(eventType)
                && EventAndHandlerMapping[eventType]?.Count > 0)
            {
                foreach (var handler in EventAndHandlerMapping[eventType])
                {
                    try
                    {
                        var method = typeof(IEventHandler<>)
                             .MakeGenericType(eventType)
                             .GetMethod(
                                 nameof(IEventHandler<IEvent>.HandleEventAsync),
                                 new[] { eventType }
                             );

                        await ((Task)method.Invoke(handler, new[] { eventData }));
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                }
            }
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
                        Subscribe(eventType, type);
                    }
                }
            }
        }

        public void Subscribe<TEvent, THandler>()
            where TEvent : IEvent
            where THandler : IEventHandler<TEvent>
        {
            Subscribe(typeof(TEvent), typeof(THandler));
        }

        public virtual void Subscribe(Type eventType, Type eventHandler)
        {
            if (EventAndHandlerMapping.ContainsKey(eventType))
            {
                List<Type> handlerTypes = EventAndHandlerMapping[eventType];

                if (handlerTypes.Contains(eventHandler)) return;

                handlerTypes.Add(eventHandler);
                EventAndHandlerMapping[eventType] = handlerTypes;
            }
            else
            {
                var handlerTypes = new List<Type> { eventHandler };
                EventAndHandlerMapping[eventType] = handlerTypes;
            }
        }

        public void Unsubscribe<TEvent, THandler>()
            where TEvent : IEvent
            where THandler : IEventHandler<TEvent>
        {
            Unsubscribe(typeof(TEvent), typeof(THandler));
        }

        /// <summary>
        /// Unsubscribe handler.
        /// ( You can leave abstract method to be implemented by subclass. )
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="handlerType"></param>
        public virtual void Unsubscribe(Type eventType, Type handlerType)
        {
            List<Type> handlerTypes = EventAndHandlerMapping[eventType];
            if (handlerTypes.Contains(handlerType))
            {
                handlerTypes.Remove(handlerType);
                EventAndHandlerMapping[eventType] = handlerTypes;
            }
        }

        protected class EventTypeWithEventHandlerFactories
        {
            public Type EventType { get; }
        }
    }
}
