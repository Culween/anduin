using System;
using System.Collections.Generic;
using System.Text;

namespace Anduin.EventBus.Events
{
    public class IntegrationEvent
    {
        public IntegrationEvent(string routeKey = null)
        {
            Id = Guid.NewGuid().ToString();
            CreationDate = DateTime.Now;
            RouteKey = routeKey ?? Id;
        }

        public string Id { get; protected set; }

        public DateTime CreationDate { get; protected set; }

        public string RouteKey { get; set; }
    }
}
