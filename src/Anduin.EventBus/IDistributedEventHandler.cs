using Anduin.EventBus.Events;
using System.Threading.Tasks;

namespace Anduin.EventBus
{
    interface IDistributedEventHandler<in TEvent> where TEvent : IntegrationEvent
    {
        Task HandleAsync(TEvent @event);
    }
}
