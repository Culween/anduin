using System;
namespace Anduin.EventBus.Kafka.Serializers
{
    public interface IKafkaSerializer
    {
        object Desrialize(Type type, byte[] bytes);

        object Desrialize<T>(byte[] bytes);

        byte[] Serialize(object obj);
    }
}
