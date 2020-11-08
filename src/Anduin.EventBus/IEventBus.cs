using System;
using System.Threading.Tasks;
using Anduin.EventBus.Events;

namespace Anduin.EventBus
{
    public interface IEventBus
    {
        void Start();

        Task PublishAsync<TEventData>(TEventData eventData)
            where TEventData : IEventData;

        Task PublishAsync(Type eventType, object eventData);

        void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IEventHandler<T>;

        void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IEventHandler<T>;
    }
}
