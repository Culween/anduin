using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anduin.Kafka
{
    [Serializable]
    public class KafkaConnections<T> : Dictionary<string, T> where T : new()
    {
        public const string DefaultConnectionName = "Default";

        public T Default
        {
            get => this[DefaultConnectionName];
            set => this[DefaultConnectionName] = (value ?? throw new ArgumentNullException(nameof(Default)));
        }

        public KafkaConnections()
        {
            Default = new T();
        }

        public T GetOrDefault(string connectionName)
        {
            if (TryGetValue(connectionName, out T connection))
            {
                return connection;
            }
            return Default;
        }
    }
}
