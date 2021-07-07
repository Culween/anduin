using System;
using MessagePack;
using MessagePack.Resolvers;

namespace Anduin.EventBus.Kafka.Serializers
{
    public class MessagePackSerializer : IKafkaSerializer
    {
        public object Desrialize(Type type, byte[] bytes)
        {
            return MessagePack.MessagePackSerializer.Deserialize(type, bytes, ContractlessStandardResolverAllowPrivate.Options);
        }

        public object Desrialize<T>(byte[] bytes)
        {
            return MessagePack.MessagePackSerializer.Deserialize<T>(bytes, ContractlessStandardResolverAllowPrivate.Options);
        }

        public byte[] Serialize(object obj)
        {
            return MessagePack.MessagePackSerializer.Serialize(obj.GetType(), obj, ContractlessStandardResolverAllowPrivate.Options);
        }
    }
}
