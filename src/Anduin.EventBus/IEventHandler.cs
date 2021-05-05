using System.Threading.Tasks;

namespace Anduin.EventBus
{
    public interface IEventHandler<in TEvent> where TEvent : IEvent
    {
        Task HandleEventAsync(TEvent @event);
    }
}
