using System;
using Anduin.EventBus.Events;

namespace Anduin.EventBus.Serializers
{
    public interface IEventSerializer
    {
        byte[] Serialize(IntegrationEvent @event);

        IntegrationEvent Desrialize(Type eventType, byte[] eventBytes);
    }
}
