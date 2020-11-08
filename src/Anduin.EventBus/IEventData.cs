using System;
using System.Collections.Generic;
using System.Text;

namespace Anduin.EventBus
{
    public interface IEventData
    {
        DateTime EventTime { get; set; }

        object EventSource { get; set; }
    }
}
