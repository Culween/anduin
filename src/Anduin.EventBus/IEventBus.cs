using Anduin.EventBus.Events;
using System;
using System.Threading.Tasks;

namespace Anduin.EventBus
{
    public interface IEventBus
    {
        Task Publish(IntegrationEvent @event, string topic = null);

        void Start();

        void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;

        void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;
    }
}
