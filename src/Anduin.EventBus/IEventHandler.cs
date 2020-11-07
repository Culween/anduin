using Anduin.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Anduin.EventBus
{
    public interface IEventHandler<in TEvent> where TEvent : IntegrationEvent
    {
        Task HandleAsync(TEvent @event);
    }
}
