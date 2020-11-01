using System;
using MessagePack;
using MessagePack.Resolvers;
using Anduin.EventBus.Events;

namespace Anduin.EventBus.Serializers
{
    public class MessagePackEventSerializer : IEventSerializer
    {
        public IntegrationEvent Desrialize(Type eventType, byte[] eventBytes)
        {
            var obj = MessagePackSerializer.Deserialize(eventType, eventBytes, ContractlessStandardResolverAllowPrivate.Options);
            return obj as IntegrationEvent;
        }

        public byte[] Serialize(IntegrationEvent @event)
        {
            return MessagePackSerializer.Serialize(@event.GetType(), @event, ContractlessStandardResolverAllowPrivate.Options);
        }
    }
}
