using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Anduin.EventBus.Local
{
    public class LocalEventBus : EventBusBase
    {
        ILogger<LocalEventBus> _logger;

        public LocalEventBus(
            ILogger<LocalEventBus> logger
            ) : base()
        {
            _logger = logger;
        }

        public override async Task PublishAsync(Type eventType, object eventData)
        {
            var exceptions = new List<Exception>();
            
            _logger.LogInformation($"Publish {nameof(eventType)}.");
            
            await TriggerHandlersAsync(eventType, eventData, exceptions);

            exceptions.ForEach((e) => _logger.LogError(e, $"Trigger handler error."));
        }
    }
}
