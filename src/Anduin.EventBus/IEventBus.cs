using Anduin.EventBus.Events;
using System;
using System.Threading.Tasks;

namespace Anduin.EventBus
{
    public interface IEventBus
    {
        void Start();

        Task Publish(IntegrationEvent @event);

        void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IEventHandler<T>;

        void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IEventHandler<T>;
    }
}
