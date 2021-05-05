using System;
using System.Collections.Generic;
using System.Text;

namespace Anduin.EventBus
{
    public interface IEvent
    {
        DateTime EventTime { get; set; }

        object EventSource { get; set; }
    }
}
