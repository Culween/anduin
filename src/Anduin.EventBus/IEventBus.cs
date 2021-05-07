using System;
using System.Threading.Tasks;

namespace Anduin.EventBus
{
    public interface IEventBus
    {
        Task PublishAsync<TEventData>(TEventData eventData)
            where TEventData : IEvent;

        Task PublishAsync(Type eventType, object eventData);

        void Subscribe<TEvent, THandler>()
            where TEvent : IEvent
            where THandler : IEventHandler<TEvent>;

        void Subscribe(Type eventType, Type handlerType);

        void Unsubscribe<TEvent, THandler>()
            where TEvent : IEvent
            where THandler : IEventHandler<TEvent>;

        void Unsubscribe(Type eventType, Type handlerType);
    }
}
