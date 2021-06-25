using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Anduin.EventBus
{
    public class EventBusBuilder
    {
        public IServiceCollection Services { get; private set; }

        internal List<Action<IEventBus>> RegisterEventHandlerActions { get; private set; } = new List<Action<IEventBus>>();

        public EventBusBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public EventBusBuilder Subscribe<TEvent, THandler>()
            where TEvent : IEvent
            where THandler : class, IEventHandler<TEvent>
        {
            Services.AddTransient<THandler>();
            RegisterEventHandlerActions.Add(ebs => ebs.Subscribe<TEvent, THandler>());

            return this;
        }
    }
}
