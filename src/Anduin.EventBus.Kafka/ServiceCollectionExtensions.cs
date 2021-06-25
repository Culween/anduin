using Anduin.EventBus.Kafka;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddKafkaEventBus(this IServiceCollection services, Action<OctopusEventBusBuilder> setupAction)
        {
            services.AddOptions<KafkaEventBusOptions>();
        }
    }
}
